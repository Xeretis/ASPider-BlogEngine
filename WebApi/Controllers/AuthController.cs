using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Auth.Authorization.Attributes;
using Auth.Services.Types;
using AutoMapper;
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
    private readonly UserManager<ApiUser> _userManager;

    public AuthController(UserManager<ApiUser> userManager,
        SignInManager<ApiUser> signInManager, IAuthService authService, IMapper mapper, IMemoryCache cache)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
        _mapper = mapper;
        _cache = cache;
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

        return Ok(new LoginResponseModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = token.ValidTo,
            User = userResponse
        });
    }

    [Authorize]
    [HttpGet("User")]
    public async Task<ActionResult<UserResponseModel>> GetUser()
    {
        var user = await _userManager.FindByIdAsync(User.FindFirstValue(JwtRegisteredClaimNames.Sub));
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
        var user = await _userManager.FindByIdAsync(User.FindFirstValue(JwtRegisteredClaimNames.Sub));

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

        if (result.Errors.Any())
            return BadRequest(result.Errors);

        user.ChangePassword = false;
        await _userManager.UpdateAsync(user);

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(
                DateTimeOffset.FromUnixTimeSeconds(
                    int.Parse(HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Exp))));

        _cache.Set($"PasswordChange{user.UserName}", false, cacheEntryOptions);

        return NoContent();
    }
}