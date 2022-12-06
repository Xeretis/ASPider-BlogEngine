using Auth;
using Auth.Authorization;
using Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class FilesController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public FilesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("{name}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Get([FromRoute] string name)
    {
        var file = await _unitOfWork.FileUploads.GetByFilenameAsync(name);

        if (file == null || !System.IO.File.Exists(Path.Combine("Resources", "Files", file.Filename)))
            return NotFound();

        return File(System.IO.File.OpenRead(Path.Combine("Resources", "Files", file.Filename)), file.ContentType,
            file.OriginalFilename);
    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var file = await _unitOfWork.FileUploads.GetByIdWithPagePostAsync(id);

        if (file == null)
            return NotFound();

        if (!HttpContext.User.IsInRole(ApiRoles.Webmaster) && !HttpContext.User.IsInRole(ApiRoles.Moderator))
            if (file.Page != null || (file.Post != null &&
                                      file.Post.AuthorId != HttpContext.User.FindFirst(AuthConstants.UserIdClaimType)!
                                          .Value))
                return Forbid();

        System.IO.File.Delete(Path.Combine("Resources", "Files", file.Filename));

        _unitOfWork.FileUploads.Remove(file);

        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
}