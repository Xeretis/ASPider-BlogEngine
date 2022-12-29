using Application.Services.Types;
using Auth.Authorization;
using Auth.Authorization.Attributes;
using AutoMapper;
using Domain.Common;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Config;
using WebApi.Models.Files;

namespace WebApi.Controllers;

[Authorize(Roles = $"{ApiRoles.Webmaster}")]
[PasswordChanged(true)]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ConfigController : Controller
{
    private readonly IConfigService _configService;
    private readonly IFileService _fileService;
    private readonly HtmlSanitizer _htmlSanitizer;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ConfigController(IConfigService configService, IMapper mapper, HtmlSanitizer htmlSanitizer,
        IFileService fileService, IUnitOfWork unitOfWork)
    {
        _configService = configService;
        _mapper = mapper;
        _htmlSanitizer = htmlSanitizer;
        _fileService = fileService;
        _unitOfWork = unitOfWork;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        return Ok(await _configService.GetConfigAsync());
    }

    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Patch([FromForm] EditConfigRequestModel model)
    {
        var config = await _configService.GetConfigAsync();

        _mapper.Map(model, config);

        config.AboutContent = _htmlSanitizer.Sanitize(model.AboutContent);

        if (model.AboutFiles != null)
            foreach (var file in model.AboutFiles)
            {
                var fileUpload = await _fileService.UploadFileAsync(file);
                _unitOfWork.Add(fileUpload);
                config.AboutFiles.Add(_mapper.Map<FileUploadResponseModel>(fileUpload));
            }

        await _configService.SetConfigAsync(config);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
}