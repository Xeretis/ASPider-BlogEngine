using Application.Services.Types;
using Auth.Authorization.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Image;

namespace WebApi.Controllers;

[Authorize]
[PasswordChanged(true)]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ImagesController : Controller
{
    private readonly IFileService _fileService;

    public ImagesController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ImageUploadResponseModel>> Upload([FromForm] ImageUploadRequestModel model)
    {
        var path = await _fileService.UploadImageAsync(model.Image);
        return Ok(new ImageUploadResponseModel
        {
            ImageUrl = $"https://localhost:7003/{path}"
        });
    }
}