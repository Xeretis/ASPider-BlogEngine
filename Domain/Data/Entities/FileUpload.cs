using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Domain.Data.Entities;

[Index(nameof(Filename), IsUnique = true)]
public class FileUpload : BaseEntity
{
    public string Filename { get; set; }
    public string OriginalFilename { get; set; }
    public string ContentType { get; set; }

    public Page? Page { get; set; }
    public Post? Post { get; set; }
}