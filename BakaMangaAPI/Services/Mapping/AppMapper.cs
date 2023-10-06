using AutoMapper;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.Services.Mapping;

public partial class AppMapper : Profile
{
    public AppMapper()
    {
        CreateMap<MangaListItem, ChapterGroupingDTO>()
            .ForMember(dest => dest.Chapters, opt => opt
                .MapFrom(src => src.Manga.Chapters.OrderBy(c => c.CreatedAt).Take(3)));
    }
}
