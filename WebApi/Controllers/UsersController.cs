using Application.Services.Types;
using Auth.Authorization;
using Auth.Authorization.Attributes;
using AutoMapper;
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
    private readonly UserManager<ApiUser> _userManager;
    private readonly IUsersService _usersService;

    public UsersController(UserManager<ApiUser> userManager, IMapper mapper, IUsersService usersService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _usersService = usersService;
    }

    [Authorize(Roles = ApiRoles.Webmaster)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsersIndexResponseModel>>> Index()
    {
        var usersWithRoles = await _usersService.GetUsersWithRolesAsync();
        var response = new List<UsersIndexResponseModel>(usersWithRoles.Keys.Count);

        foreach (var userWithRole in usersWithRoles)
        {
            var model = _mapper.Map<UsersIndexResponseModel>(userWithRole.Key);
            model.Roles = userWithRole.Value.Select(e => e.Name).ToList();
            response.Add(model);
        }

        return Ok(response);
    }
}