using WebApi.Models.Files;

namespace WebApi.Models.Approve;

public class ViewApproveResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }

    public string? ThumbnailUrl { get; set; }

    public bool Visible { get; set; }
    public bool Approved { get; set; }

    public int PageId { get; set; }
    public ViewApproveUserResponseModel Author { get; set; }

    public List<FileUploadResponseModel> Files { get; set; }
}

public class ViewApproveUserResponseModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string? ProfilePictureUrl { get; set; }
}