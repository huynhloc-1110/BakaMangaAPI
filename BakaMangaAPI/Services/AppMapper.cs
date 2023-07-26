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
            .ForMember(dest => dest.RatingSum, opt => opt
                .MapFrom(src => src.Ratings.Sum(r => r.Value)))
            .ForMember(dest => dest.RatingCount, opt => opt
                .MapFrom(src => src.Ratings.Count()))
            .ForMember(dest => dest.FollowCount, opt => opt
                .MapFrom(src => src.Followers.Count));
        CreateMap<MangaEditDTO, Manga>();

        CreateMap<Chapter, ChapterBasicDTO>()
            .ForMember(dest => dest.ViewCount, opt => opt
                .MapFrom(src => src.ChapterViews.Count));

        CreateMap<ApplicationUser, UserBasicDTO>()
            .ForMember(dest => dest.Roles, opt => opt
                .MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)));
        CreateMap<ApplicationUser, UserSimpleDTO>();

        CreateMap<Author, AuthorDTO>();
        CreateMap<AuthorEditDTO, Author>();

        CreateMap<Category, CategoryDTO>();
        CreateMap<CategoryEditDTO, Category>();

        CreateMap<MangaComment, CommentDTO>()
            .ForMember(dest => dest.ChildCommentCount, opt => opt
                .MapFrom(src => src.ChildComments.Count))
            .ForMember(dest => dest.LikeCount, opt => opt
                .MapFrom(src => src.Reacts.Count(r => r.ReactFlag == ReactFlag.Like)))
            .ForMember(dest => dest.DislikeCount, opt => opt
                .MapFrom(src => src.Reacts.Count(r => r.ReactFlag == ReactFlag.Dislike)));
    }
}
