using AutoMapper;
using BakaMangaAPI.Models;
using BakaMangaAPI.DTOs;

namespace BakaMangaAPI.Services;

public class AppMapper : Profile
{
    public AppMapper()
    {
        CreateMap<Manga, MangaBasicDTO>();
        CreateMap<Manga, MangaDetailDTO>();
        CreateMap<MangaEditDTO, Manga>();

        CreateMap<Chapter, ChapterBasicDTO>();

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
