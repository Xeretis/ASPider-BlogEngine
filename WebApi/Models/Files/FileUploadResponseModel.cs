namespace WebApi.Models.Files;

public class FileUploadResponseModel
{
    public string Filename { get; set; }
    public string OriginalFilename { get; set; }
    public string ContentType { get; set; }
    public string Url { get; set; }
}