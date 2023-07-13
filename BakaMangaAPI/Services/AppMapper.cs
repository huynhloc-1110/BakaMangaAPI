using AutoMapper;
using BakaMangaAPI.Models;
using BakaMangaAPI.DTOs;

namespace BakaMangaAPI.Services;

public class AppMapper : Profile
{
    public AppMapper()
    {
        CreateMap<Manga, MangaBasicDTO>();
        CreateMap<Manga, MangaDetailDTO>()
            .ForMember(dest => dest.AverageRating, opt => opt
                .MapFrom(src => src.Ratings.Any() ? src.Ratings.Average(r => r.Value) : 3))
            .ForMember(dest => dest.FollowCount, opt => opt
                .MapFrom(src => src.Followers.Count));
        CreateMap<MangaEditDTO, Manga>();

        CreateMap<Chapter, ChapterBasicDTO>()
            .ForMember(dest => dest.ViewCount, opt => opt
                .MapFrom(src => src.ChapterViews.Count()));

        CreateMap<ApplicationUser, UserBasicDTO>()
            .ForMember(dest => dest.Roles, opt => opt
                .MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)));
        CreateMap<ApplicationUser, UserSimpleDTO>();

        CreateMap<Author, AuthorDTO>();
        CreateMap<AuthorEditDTO, Author>();

        CreateMap<Category, CategoryDTO>();
        CreateMap<CategoryEditDTO, Category>();
    }
}
