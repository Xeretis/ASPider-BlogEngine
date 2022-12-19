using Auth.Authorization.Attributes;
using AutoMapper;
using Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Posts;

namespace WebApi.Controllers;

[Authorize]
[PasswordChanged(true)]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class PostsController : Controller
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public PostsController(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ViewPostResponseModel>> View([FromRoute] int id)
    {
        var post = await _unitOfWork.Posts.GetByIdWithFilesAsync(id);

        if (post == null || !post.Visible || !post.Approved)
            return NotFound();

        var model = _mapper.Map<ViewPostResponseModel>(post);

        return Ok(model);
    }
}