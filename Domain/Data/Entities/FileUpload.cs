using Domain.Common;

namespace Domain.Data.Entities;

public class FileUpload : BaseEntity
{
    public string Filename { get; set; }
    public string OriginalFilename { get; set; }
    public string ContentType { get; set; }
}