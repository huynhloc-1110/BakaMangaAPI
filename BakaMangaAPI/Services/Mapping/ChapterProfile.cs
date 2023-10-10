using AutoMapper;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.Services.Mapping;

public class ChapterProfile : Profile
{
    public ChapterProfile()
    {
        string? currentUserId = null;

        CreateMap<Chapter, ChapterBasicDTO>()
            .ForMember(dest => dest.ViewCount, opt => opt
                .MapFrom(src => src.ChapterViews.Count))
            .ForMember(dest => dest.IsViewedByUser, opt => opt
                .MapFrom(src => src.ChapterViews.Select(v => v.UserId).Contains(currentUserId)));

        CreateMap<Chapter, ChapterDetailDTO>()
            .ForMember(dest => dest.PageUrls, opt => opt
                .MapFrom(src => src.Images.OrderBy(p => p.Number).Select(p => p.Path).ToList()));

        CreateMap<Chapter, ChapterSimpleDTO>();
    }
}
