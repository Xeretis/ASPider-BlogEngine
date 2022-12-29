using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Auth;
using Auth.Authorization.Attributes;
using Auth.Services.Types;
using AutoMapper;
using Domain.Common;
using Domain.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebApi.Models.Auth;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly IMemoryCache _cache;
    private readonly IMapper _mapper;
    private readonly SignInManager<ApiUser> _signInManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApiUser> _userManager;

    public AuthController(UserManager<ApiUser> userManager,
        SignInManager<ApiUser> signInManager, IAuthService authService, IMapper mapper, IMemoryCache cache,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
        _mapper = mapper;
        _cache = cache;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponseModel>> Login([FromBody] LoginRequestModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);

        if (user == null)
        {
            ModelState.AddModelError(nameof(LoginRequestModel.Username), "Username or password is invalid");
            return ValidationProblem();
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password,
            await _userManager.GetLockoutEnabledAsync(user));

        if (result == SignInResult.Failed)
        {
            ModelState.AddModelError(nameof(LoginRequestModel.Username), "Username or password is invalid");
            return ValidationProblem();
        }

        if (result == SignInResult.LockedOut)
        {
            ModelState.AddModelError(nameof(LoginRequestModel.Username), "This user is locked out");
            return ValidationProblem();
        }

        if (_userManager.SupportsUserLockout && await _userManager.GetAccessFailedCountAsync(user) > 0)
            await _userManager.ResetAccessFailedCountAsync(user);

        var authClaims = await _authService.GetAuthClaims(user);
        var token = _authService.GetAuthToken(authClaims);

        var userResponse = _mapper.Map<LoginResponseUserModel>(user);
        userResponse.Roles = authClaims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

        if (model.Remember)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(AuthConstants.RefreshTokenExpirationDays),
                HttpOnly = true,
                IsEssential = true,
                Secure = true
            };
            Response.Cookies.Append("refreshToken", await _authService.GetRefreshTokenAsync(user.Id),
                cookieOptions);
        }

        return Ok(new LoginResponseModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = token.ValidTo,
            User = userResponse
        });
    }

    [HttpPost("Refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponseModel>> Refresh()
    {
        var token = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(token))
        {
            ModelState.AddModelError("refreshToken", "Refresh token is missing");
            return ValidationProblem();
        }

        var refreshToken = await _unitOfWork.RefreshTokens.GetByTokenWithUserAsync(token);
        if (refreshToken == null ||
            refreshToken.CreatedDate.AddDays(AuthConstants.RefreshTokenExpirationDays) < DateTime.Now)
        {
            ModelState.AddModelError("refreshToken", "Refresh token is invalid");
            return ValidationProblem();
        }

        var newRefreshToken = await _authService.GetRefreshTokenAsync(refreshToken.User!.Id);

        var authClaims = await _authService.GetAuthClaims(refreshToken.User);
        var authToken = _authService.GetAuthToken(authClaims);

        var userResponse = _mapper.Map<LoginResponseUserModel>(refreshToken.User);
        userResponse.Roles = authClaims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(AuthConstants.RefreshTokenExpirationDays),
            HttpOnly = true,
            IsEssential = true,
            Secure = true
        };
        Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions);

        _unitOfWork.RefreshTokens.Remove(refreshToken);
        await _unitOfWork.CompleteAsync();

        return Ok(new LoginResponseModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(authToken),
            ExpiresAt = authToken.ValidTo,
            User = userResponse
        });
    }

    [Authorize]
    [HttpGet("User")]
    public async Task<ActionResult<UserResponseModel>> GetUser()
    {
        var user = await _userManager.FindByIdAsync(User.FindFirstValue(AuthConstants.UserIdClaimType)!);
        return Ok(_mapper.Map<UserResponseModel>(user));
    }

    [Authorize]
    [HttpGet("Roles")]
    public ActionResult<IEnumerable<string>> GetRoles()
    {
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        return Ok(roles);
    }

    [Authorize]
    [PasswordChanged(false)]
    [HttpPost("ChangePassword")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequestModel model)
    {
        var user = await _userManager.FindByIdAsync(User.FindFirstValue(AuthConstants.UserIdClaimType)!);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

        if (result.Errors.Any())
            return BadRequest(result.Errors);

        user.ChangePassword = false;
        await _userManager.UpdateAsync(user);

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(
                DateTimeOffset.FromUnixTimeSeconds(
                    int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Exp)!)));

        _cache.Set($"PasswordChange{user.UserName}", false, cacheEntryOptions);

        return NoContent();
    }
}