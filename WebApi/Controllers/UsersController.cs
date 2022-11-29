using System.Security.Claims;
using Auth;
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
    [HttpPost]
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

    [Authorize(Roles = ApiRoles.Webmaster)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] string id) //TODO: Make the ghost user function
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null || user.Id == AuthConstants.GhostUserId) return NotFound();

        if (user.Id == AuthConstants.DefaultWebmasterId) return BadRequest("Cannot delete default webmaster");

        var deleteResult = await _userManager.DeleteAsync(user);
        if (!deleteResult.Succeeded) return BadRequest(deleteResult.Errors);

        return NoContent();
    }

    [Authorize(Roles = ApiRoles.Webmaster)]
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Edit([FromRoute] string id, [FromBody] EditUserRequestModel model)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null || user.Id == AuthConstants.GhostUserId) return NotFound();

        _mapper.Map(model, user);

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded) return BadRequest(updateResult.Errors);

        if (model.Password != null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, token, model.Password);
            if (!resetResult.Succeeded) return BadRequest(resetResult.Errors);
        }

        return NoContent();
    }

    [HttpPatch("Self")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> EditSelf([FromBody] EditSelfRequestModel model)
    {
        var user = await _userManager.FindByIdAsync(User.FindFirstValue(AuthConstants.UserIdClaimType));

        _mapper.Map(model, user);

        if (!await _userManager.CheckPasswordAsync(user, model.Password))
            return BadRequest("Current password is incorrect");

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded) return BadRequest(updateResult.Errors);

        if (model.NewPassword != null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (!resetResult.Succeeded) return BadRequest(resetResult.Errors);
        }

        return NoContent();
    }
}