using System.Security.Claims;
using Application.Services.Types;
using Auth;
using Auth.Authorization;
using Domain.Data.Entities;

namespace Application.Services.Implementations;

public class PostService : IPostService
{
    public bool IsModifyAllowed(ClaimsPrincipal user, Post post)
    {
        return user.IsInRole(ApiRoles.Webmaster) || user.IsInRole(ApiRoles.Moderator) ||
               post.AuthorId == user.FindFirstValue(AuthConstants.UserIdClaimType);
    }

    public bool IsContentModified(IPostService.IPostModification modification, Post post)
    {
        return modification.Title != post.Title || modification.Description != post.Description ||
               modification.Content != post.Content ||
               (modification.ThumbnailUrl != null && modification.ThumbnailUrl != post.ThumbnailUrl) ||
               modification.PageId != post.PageId;
    }
}