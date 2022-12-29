using Auth.Authorization;
using Auth.Authorization.Attributes;
using AutoMapper;
using Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Approve;
using WebApi.Models.Posts;

namespace WebApi.Controllers;

[Authorize(Roles = $"{ApiRoles.Webmaster},{ApiRoles.Moderator}")]
[PasswordChanged(true)]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ApproveController : Controller
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<IndexApproveResponseModel>>> Index()
    {
        var posts = await _unitOfWork.Posts.GetUnapprovedWithPageAuthorFiles();
        var response = _mapper.Map<IEnumerable<IndexPostResponseModel>>(posts);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ViewApproveResponseModel>> View([FromRoute] int id)
    {
        var post = await _unitOfWork.Posts.GetByIdWithAuthorFilesAsync(id);

        if (post == null)
            return NotFound();

        var response = _mapper.Map<ViewApproveResponseModel>(post);
        return Ok(response);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Edit([FromRoute] int id, [FromBody] EditApproveRequestModel model)
    {
        var post = await _unitOfWork.Posts.GetByIdAsync(id);

        if (post == null)
            return NotFound();

        _mapper.Map(model, post);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
}