using AutoMapper;
using Domain.Data.Entities;
using WebApi.Models.Pages;

namespace WebApi.MappingProfiles.Resolvers;

public class SubpageResolver : IValueResolver<Page, ViewPageResponseModel, List<ViewPageResponseModel>>
{
    public List<ViewPageResponseModel> Resolve(Page source, ViewPageResponseModel destination,
        List<ViewPageResponseModel> destMember,
        ResolutionContext context)
    {
        if (source.Children == null)
            return new List<ViewPageResponseModel>();
        return source.Children.Where(c => c.Visible).Select(c => context.Mapper.Map<ViewPageResponseModel>(c))
            .ToList();
    }
}