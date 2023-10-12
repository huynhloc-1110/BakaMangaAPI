using AutoMapper;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.Services.Mapping;

public class MangaListProfile : Profile
{
    public MangaListProfile()
    {
        string? checkedMangaId = null;
        string? currentUserId = null;
        CreateMap<MangaList, MangaListBasicDTO>()
            .ForMember(dest => dest.MangaCoverUrls, opt => opt
                .MapFrom(src => src.Items
                    .OrderBy(i => i.Index)
                    .Select(i => i.Manga.CoverPath)
                    .Take(3)))
            .ForMember(dest => dest.UpdatedAt, opt => opt
                .MapFrom(src => src.Items.Any() ? src.Items.Max(i => i.AddedAt) : src.CreatedAt))
            .ForMember(dest => dest.AlreadyAdded, opt => opt
                .MapFrom(src => src.Items.Select(i => i.MangaId).Contains(checkedMangaId)))
            .ForMember(dest => dest.AlreadyFollowed, opt => opt
                .MapFrom(src => src.Followers.Select(f => f.UserId).Contains(currentUserId)));

        CreateMap<MangaListEditDTO, MangaList>();
    }
}
