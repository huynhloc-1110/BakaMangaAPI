using AutoMapper;
using BakaMangaAPI.Models;
using BakaMangaAPI.DTOs;

namespace BakaMangaAPI.Services;

public class AppMapper : Profile
{
    public AppMapper()
    {
        CreateMap<Manga, MangaBasicDTO>();
        CreateMap<Manga, MangaDetailDTO>().ReverseMap();

        CreateMap<Chapter, ChapterBasicDTO>();

        CreateMap<ApplicationUser, UserBasicDTO>();
    }
}
