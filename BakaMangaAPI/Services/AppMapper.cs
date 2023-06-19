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

        CreateMap<ApplicationUser, UserBasicDTO>();

        CreateMap<Author, AuthorBasicDTO>();
        CreateMap<Author, AuthorDetailDTO>().ReverseMap();

        CreateMap<Category, CategoryBasicDTO>();
        CreateMap<Category, CategoryDetailDTO>().ReverseMap();

    }
}
