using AutoMapper;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.Services.Mapping;

public partial class AppMapper : Profile
{
    public AppMapper()
    {   
        CreateMap<Chapter, ChapterBasicDTO>()
            .ForMember(dest => dest.ViewCount, opt => opt
                .MapFrom(src => src.ChapterViews.Count));
        CreateMap<Chapter, ChapterDetailDTO>()
            .ForMember(dest => dest.PageUrls, opt => opt
                .MapFrom(src => src.Images.OrderBy(p => p.Number).Select(p => p.Path).ToList()));
        CreateMap<Chapter, ChapterSimpleDTO>();

        CreateMap<MangaListItem, ChapterGroupingDTO>()
            .ForMember(dest => dest.Chapters, opt => opt
                .MapFrom(src => src.Manga.Chapters.OrderBy(c => c.CreatedAt).Take(3)));
    }
}
