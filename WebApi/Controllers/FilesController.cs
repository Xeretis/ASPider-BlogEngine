using Domain.Common;
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
}