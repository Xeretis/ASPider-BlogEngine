using System.Security.Claims;
using Application.Services.Types;
using Auth;
using Auth.Authorization;
using Auth.Authorization.Attributes;
using AutoMapper;
using Domain.Common;
using Domain.Data.Entities;
using Ganss.Xss;
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
    private readonly IFileService _fileService;
    private readonly HtmlSanitizer _htmlSanitizer;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public PostsController(IMapper mapper, IUnitOfWork unitOfWork, IFileService fileService,
        HtmlSanitizer htmlSanitizer)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _htmlSanitizer = htmlSanitizer;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<IndexPostResponseModel>>> Index()
    {
        List<Post> posts;
        if (User.IsInRole(ApiRoles.Webmaster) || User.IsInRole(ApiRoles.Moderator))
            posts = await _unitOfWork.Posts.GetAllWithPageAuthorFiles();
        else
            posts = await _unitOfWork.Posts.GetFromUserWithPageAuthorFiles(
                User.FindFirstValue(AuthConstants.UserIdClaimType)!);

        var response = _mapper.Map<IEnumerable<IndexPostResponseModel>>(posts);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ViewPostResponseModel>> View([FromRoute] int id)
    {
        var post = await _unitOfWork.Posts.GetByIdWithAuthorFilesAsync(id);

        if (post == null || !post.Visible || !post.Approved)
            return NotFound();

        var model = _mapper.Map<ViewPostResponseModel>(post);

        return Ok(model);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create([FromForm] CreatePostRequestModel model)
    {
        var parentPage = await _unitOfWork.Pages.GetByIdWithChildrenAsync(model.PageId);

        if (parentPage == null)
        {
            ModelState.AddModelError(nameof(model.PageId), "Page not found");
            return ValidationProblem();
        }

        var post = _mapper.Map<Post>(model);
        post.Files = new List<FileUpload>();
        post.AuthorId = User.FindFirstValue(AuthConstants.UserIdClaimType)!;

        post.Content = _htmlSanitizer.Sanitize(model.Content);

        if (User.IsInRole(ApiRoles.Webmaster) || User.IsInRole(ApiRoles.Moderator))
            post.Approved = true;

        if (model.Files != null)
            foreach (var file in model.Files)
                post.Files.Add(await _fileService.UploadFileAsync(file));

        _unitOfWork.Add(post);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var post = await _unitOfWork.Posts.GetByIdWithFilesAsync(id);

        if (post == null)
            return NotFound();

        if (!(User.IsInRole(ApiRoles.Webmaster) || User.IsInRole(ApiRoles.Moderator)) &&
            post.AuthorId != User.FindFirstValue(AuthConstants.UserIdClaimType))
            return Forbid();

        foreach (var file in post.Files!) System.IO.File.Delete(Path.Combine("Resources", "Files", file.Filename));

        _unitOfWork.Posts.Remove(post);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
}