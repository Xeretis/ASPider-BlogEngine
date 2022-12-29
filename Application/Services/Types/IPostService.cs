using System.Security.Claims;
using Domain.Data.Entities;

namespace Application.Services.Types;

public interface IPostService
{
    bool IsModifyAllowed(ClaimsPrincipal user, Post post);

    bool IsContentModified(IPostModification modification, Post post);

    public interface IPostModification
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }

        public string? ThumbnailUrl { get; set; }

        public int PageId { get; set; }
    }
}