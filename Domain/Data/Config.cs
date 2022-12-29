namespace Domain.Data;

public class Config
{
    public string Title { get; set; } = "ASPider Blogengine";
    public string Description { get; set; } = "ASPider Blogengine Description";
    public string AboutContent { get; set; } = "<h1>ASPider Blogengine About Content</h1>";
    public string? PageIconUrl { get; set; }
    public string? FaviconUrl { get; set; }
    public List<IFileModel> AboutFiles { get; set; } = new();
    public string ContactEmail { get; set; } = "example@example.com";
    public string ContactPhone { get; set; } = "+36 30 676 6969";
    public string ContactName { get; set; } = "John Doe";
}

public interface IFileModel
{
    public int Id { get; set; }
    public string Filename { get; set; }
    public string OriginalFilename { get; set; }
    public string ContentType { get; set; }
    public string Url { get; set; }
}