using AutoMapper;
using Domain.Data.Entities;
using WebApi.Models.Pages;

namespace WebApi.MappingProfiles.Resolvers;

public class PostResolver : IValueResolver<Page, ViewPageResponseModel, List<ViewPagePostResponseModel>>
{
    public List<ViewPagePostResponseModel> Resolve(Page source, ViewPageResponseModel destination,
        List<ViewPagePostResponseModel> destMember, ResolutionContext context)
    {
        if (source.Posts == null) return new List<ViewPagePostResponseModel>();
        return source.Posts.Where(p => p.Visible && p.Approved)
            .Select(p => context.Mapper.Map<ViewPagePostResponseModel>(p))
            .ToList();
    }
}