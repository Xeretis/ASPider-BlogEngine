using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Auth.Services.Types;
using AutoMapper;
using Domain.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Auth;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly SignInManager<ApiUser> _signInManager;
    private readonly UserManager<ApiUser> _userManager;

    public AuthController(UserManager<ApiUser> userManager,
        SignInManager<ApiUser> signInManager, IAuthService authService, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
        _mapper = mapper;
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

        return Ok(new LoginResponseModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = token.ValidTo,
            User = _mapper.Map<LoginResponseUserModel>(user),
            Roles = authClaims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList()
        });
    }

    [Authorize]
    [HttpGet("User")]
    public async Task<ActionResult<UserResponseModel>> GetUser()
    {
        var user = await _userManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
        return Ok(_mapper.Map<UserResponseModel>(user));
    }

    [Authorize]
    [HttpGet("Roles")]
    public ActionResult<IEnumerable<string>> GetRoles()
    {
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        return Ok(roles);
    }
}