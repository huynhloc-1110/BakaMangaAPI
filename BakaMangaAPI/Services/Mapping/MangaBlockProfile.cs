using AutoMapper;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.Services.Mapping;

public partial class MangaBlockProfile : Profile
{
    public MangaBlockProfile()
    {
        CreateMap<Manga, MangaBlockDTO>()
            .ForMember(dest => dest.Chapters, opt => opt
                .MapFrom(src => src.Chapters.OrderByDescending(c => c.CreatedAt).Take(3)))
            .ForMember(dest => dest.UpdatedAt, opt => opt
                .MapFrom(src => src.Chapters.Max(c => c.CreatedAt)));

        string? uploaderId = null;
        CreateMap<Manga, UploaderMangaBlockDTO>()
            .ForMember(dest => dest.Chapters, opt => opt
                .MapFrom(src => src.Chapters
                    .Where(c => c.Uploader.Id == uploaderId)
                    .OrderByDescending(c => c.CreatedAt).Take(3)))
            .ForMember(dest => dest.UpdatedAt, opt => opt
                .MapFrom(src => src.Chapters
                    .Where(c => c.Uploader.Id == uploaderId)
                    .Max(c => c.CreatedAt)));

        string? groupId = null;
        CreateMap<Manga, GroupMangaBlockDTO>()
            .ForMember(dest => dest.Chapters, opt => opt
                .MapFrom(src => src.Chapters
                    .Where(c => c.UploadingGroup!.Id == groupId)
                    .OrderByDescending(c => c.CreatedAt).Take(3)))
            .ForMember(dest => dest.UpdatedAt, opt => opt
                .MapFrom(src => src.Chapters
                    .Where(c => c.UploadingGroup!.Id == groupId)
                    .Max(c => c.CreatedAt)));
    }
}
