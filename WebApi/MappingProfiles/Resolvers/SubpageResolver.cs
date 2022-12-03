using AutoMapper;
using Domain.Data.Entities;
using WebApi.Models.Pages;

namespace WebApi.MappingProfiles.Resolvers;

public class SubpageResolver : IValueResolver<Page, ViewPageResponse, List<ViewPageResponse>>
{
    public List<ViewPageResponse> Resolve(Page source, ViewPageResponse destination, List<ViewPageResponse> destMember,
        ResolutionContext context)
    {
        if (source.Children == null)
            return new List<ViewPageResponse>();
        return source.Children.Where(c => c.Visible).Select(c => context.Mapper.Map<ViewPageResponse>(c))
            .ToList();
    }
}