using WebApi.Models.Files;

namespace WebApi.Models.Pages;

public class ViewPageResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }

    public bool Visible { get; set; }
    public string? ThumbnailUrl { get; set; }

    public int? ParentId { get; set; }
    public string? CreatorId { get; set; }

    public List<ViewPageResponseModel> SubPages { get; set; }
    public List<ViewPagePostResponseModel> Posts { get; set; }
    public List<FileUploadResponseModel> Files { get; set; }
}

public class ViewPagePostResponseModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public ViewPageUserResponseModel Author { get; set; }
}

public class ViewPageUserResponseModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string? ProfilePictureUrl { get; set; }
}