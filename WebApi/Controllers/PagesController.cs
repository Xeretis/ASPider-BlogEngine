using System.Security.Claims;
using Application.Services.Types;
using Auth;
using Auth.Authorization;
using Auth.Authorization.Attributes;
using AutoMapper;
using Domain.Common;
using Domain.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Pages;

namespace WebApi.Controllers;

[Authorize]
[PasswordChanged(true)]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class PagesController : Controller
{
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public PagesController(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
    }

    [Authorize(Roles = $"{ApiRoles.Webmaster},{ApiRoles.Moderator}")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IndexPageResponseModel>>> Index()
    {
        var pages = await _unitOfWork.Pages.GetAllWithCreatorFilesAsync();
        var response = _mapper.Map<IEnumerable<IndexPageResponseModel>>(pages);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ViewPageResponseModel>> View([FromRoute] int id)
    {
        var page = await _unitOfWork.Pages.GetByIdWithPostsFilesChildrenAsync(id);

        if (page == null || !page.Visible)
            return NotFound();

        var model = _mapper.Map<ViewPageResponseModel>(page);

        return Ok(model);
    }

    [Authorize(Roles = $"{ApiRoles.Webmaster},{ApiRoles.Moderator}")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create([FromForm] CreatePageRequestModel model)
    {
        var parentPage = await _unitOfWork.Pages.GetByIdWithChildrenAsync(model.ParentId);

        if (parentPage == null)
        {
            ModelState.AddModelError(nameof(model.ParentId), "Parent page not found");
            return ValidationProblem();
        }

        if (await _unitOfWork.Pages.GetDepthAsync(model.ParentId) >= 4)
        {
            ModelState.AddModelError(nameof(model.ParentId), "Maximum page depth reached on parent");
            return ValidationProblem();
        }

        var page = _mapper.Map<Page>(model);
        page.Files = new List<FileUpload>();
        page.CreatorId = HttpContext.User.FindFirstValue(AuthConstants.UserIdClaimType);
        parentPage.Children!.Add(page);

        if (model.Files != null)
            foreach (var file in model.Files)
                page.Files.Add(await _fileService.UploadFileAsync(file));

        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

    [Authorize(Roles = $"{ApiRoles.Webmaster},{ApiRoles.Moderator}")]
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Edit([FromRoute] int id, [FromForm] EditPageRequestModel model)
    {
        var page = await _unitOfWork.Pages.GetByIdWithFilesAsync(id);

        if (page == null)
            return NotFound();

        if (page.ParentId != model.ParentId)
        {
            var parentPage = await _unitOfWork.Pages.GetByIdWithChildrenAsync(model.ParentId);

            if (parentPage == null)
            {
                ModelState.AddModelError(nameof(model.ParentId), "Parent page not found");
                return ValidationProblem();
            }

            if (await _unitOfWork.Pages.GetDepthAsync(model.ParentId) >= 4)
            {
                ModelState.AddModelError(nameof(model.ParentId), "Maximum page depth reached on parent");
                return ValidationProblem();
            }

            parentPage.Children!.Add(page);
        }

        if (model.Files != null)
            foreach (var file in model.Files)
                page.Files!.Add(await _fileService.UploadFileAsync(file));

        if (model.ThumbnailUrl != null)
            page.ThumbnailUrl = model.ThumbnailUrl;

        _mapper.Map(model, page);

        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

    [Authorize(Roles = $"{ApiRoles.Webmaster},{ApiRoles.Moderator}")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var page = await _unitOfWork.Pages.GetByIdWithFilesChildrenAsync(id);

        if (page == null)
            return NotFound();

        if (page.Id == 1)
        {
            ModelState.AddModelError(nameof(id), "You cannot delete the root page");
            return ValidationProblem();
        }

        if (page.Children!.Any())
        {
            ModelState.AddModelError(nameof(id), "Pages with children cannot be deleted");
            return ValidationProblem();
        }

        foreach (var file in page.Files!) System.IO.File.Delete(Path.Combine("Resources", "Files", file.Filename));

        _unitOfWork.Pages.Remove(page);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
}