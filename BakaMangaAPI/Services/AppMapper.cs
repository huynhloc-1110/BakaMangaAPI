using AutoMapper;
using BakaMangaAPI.Models;
using BakaMangaAPI.DTOs;

namespace BakaMangaAPI.Services;

public class AppMapper : Profile
{
    public AppMapper()
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
