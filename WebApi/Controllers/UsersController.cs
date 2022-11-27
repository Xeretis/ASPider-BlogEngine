using Auth.Authorization;
using Auth.Authorization.Attributes;
using AutoMapper;
using Domain.Common;
using Domain.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Users;

namespace WebApi.Controllers;

[Authorize]
[PasswordChanged(true)]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class UsersController : Controller
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApiUser> _userManager;

    public UsersController(UserManager<ApiUser> userManager, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [Authorize(Roles = ApiRoles.Webmaster)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsersIndexResponseModel>>> Index()
    {
        var usersWithRoles = await _unitOfWork.Users.GetUsersWithRolesAsync();
        var response = new List<UsersIndexResponseModel>(usersWithRoles.Keys.Count);

        foreach (var userWithRole in usersWithRoles)
        {
            var model = _mapper.Map<UsersIndexResponseModel>(userWithRole.Key);
            model.Roles = userWithRole.Value.Select(e => e.Name).ToList();
            response.Add(model);
        }

        return Ok(response);
    }

    [Authorize(Roles = ApiRoles.Webmaster)]
    [HttpPost("Create")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create([FromBody] CreateUserRequestModel model)
    {
        var user = _mapper.Map<ApiUser>(model);
        var createResult = await _userManager.CreateAsync(user, model.Password);

        if (!createResult.Succeeded) return BadRequest(createResult.Errors);

        var roleAddResult = await _userManager.AddToRoleAsync(user, model.Role);

        if (!roleAddResult.Succeeded) return BadRequest(roleAddResult.Errors);

        return NoContent();
    }
}