using AutoMapper;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.Services.Mapping;

public class MangaProfile : Profile
{
    public MangaProfile()
    {
        CreateMap<Manga, MangaSimpleDTO>();

        CreateMap<Manga, MangaBasicDTO>()
            .ForMember(dest => dest.Categories, opt => opt
                .MapFrom(src => src.Categories.Where(c => c.DeletedAt == null)));

        CreateMap<Manga, MangaDetailDTO>()
            .ForMember(dest => dest.Categories, opt => opt
                .MapFrom(src => src.Categories.Where(c => c.DeletedAt == null)))
            .ForMember(dest => dest.Authors, opt => opt
                .MapFrom(src => src.Authors.Where(c => c.DeletedAt == null)));

        CreateMap<Manga, MangaStatsDTO>()
            .ForMember(dest => dest.ViewCount, opt => opt
                .MapFrom(src => src.Chapters.Sum(c => c.ChapterViews.Count)))
            .ForMember(dest => dest.FollowCount, opt => opt
                .MapFrom(src => src.Followers.Count))
            .ForMember(dest => dest.RatingSum, opt => opt
                .MapFrom(src => src.Ratings.Sum(r => r.Value)))
            .ForMember(dest => dest.RatingCount, opt => opt
                .MapFrom(src => src.Ratings.Count));

        CreateMap<MangaEditDTO, Manga>();

        CreateMap<Manga, MangaBlockDTO>()
            .ForMember(dest => dest.Chapters, opt => opt
                .MapFrom(src => src.Chapters.OrderByDescending(c => c.CreatedAt).Take(3)))
            .ForMember(dest => dest.UpdatedAt, opt => opt
                .MapFrom(src => src.Chapters.Max(c => c.CreatedAt)));
    }
}
