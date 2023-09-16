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
                .MapFrom(src => src.Pages.OrderBy(p => p.Number).Select(p => p.Path).ToList()));
        CreateMap<Chapter, ChapterSimpleDTO>();

        CreateMap<ApplicationUser, UserBasicDTO>()
            .ForMember(dest => dest.Roles, opt => opt
                .MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)));
        CreateMap<ApplicationUser, UserSimpleDTO>();

        CreateMap<Author, AuthorDTO>();
        CreateMap<AuthorEditDTO, Author>();

        CreateMap<Category, CategoryDTO>();
        CreateMap<CategoryEditDTO, Category>();

        CreateMap<Comment, CommentDTO>()
            .ForMember(dest => dest.ChildCommentCount, opt => opt
                .MapFrom(src => src.ChildComments.Count(c => c.DeletedAt == null)))
            .ForMember(dest => dest.LikeCount, opt => opt
                .MapFrom(src => src.Reacts.Count(r => r.ReactFlag == ReactFlag.Like)))
            .ForMember(dest => dest.DislikeCount, opt => opt
                .MapFrom(src => src.Reacts.Count(r => r.ReactFlag == ReactFlag.Dislike)));
        CreateMap<CommentEditDTO, MangaComment>();
        CreateMap<CommentEditDTO, ChapterComment>();
        CreateMap<CommentEditDTO, Comment>();

        CreateMap<Group, GroupBasicDTO>();

        CreateMap<MangaListItem, ChapterGroupingDTO>()
            .ForMember(dest => dest.Chapters, opt => opt
                .MapFrom(src => src.Manga.Chapters.OrderBy(c => c.CreatedAt).Take(3)));
    }
}
