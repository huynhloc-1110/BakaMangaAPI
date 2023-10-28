using AutoMapper;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.Services.Mapping;

public class PostProfile : Profile
{
    public PostProfile()
    {
        string? currentUserId = null;
        CreateMap<Post, PostBasicDTO>()
            .ForMember(dest => dest.ImageUrls, opt => opt
                .MapFrom(src => src.Images.Select(i => i.Path)))
            .ForMember(dest => dest.LikeCount, opt => opt
                .MapFrom(src => src.Reacts.Count(r => r.ReactFlag == ReactFlag.Like)))
            .ForMember(dest => dest.DislikeCount, opt => opt
                .MapFrom(src => src.Reacts.Count(r => r.ReactFlag == ReactFlag.Dislike)))
            .ForMember(dest => dest.CommentCount, opt => opt
                .MapFrom(src => src.Comments.Count(c => c.DeletedAt == null)))
            .ForMember(dest => dest.UserReactFlag, opt => opt
                .MapFrom(src => src.Reacts
                    .Where(r => r.UserId == currentUserId)
                    .Select(r => r.ReactFlag)
                    .SingleOrDefault()))
            .ForMember(dest => dest.Group, opt => opt.
                MapFrom(src => ((GroupPost)src).Group));

        CreateMap<UserPost, PostBasicDTO>()
            .ForMember(dest => dest.ImageUrls, opt => opt
                .MapFrom(src => src.Images.Select(i => i.Path)))
            .ForMember(dest => dest.LikeCount, opt => opt
                .MapFrom(src => src.Reacts.Count(r => r.ReactFlag == ReactFlag.Like)))
            .ForMember(dest => dest.DislikeCount, opt => opt
                .MapFrom(src => src.Reacts.Count(r => r.ReactFlag == ReactFlag.Dislike)))
            .ForMember(dest => dest.CommentCount, opt => opt
                .MapFrom(src => src.Comments.Count(c => c.DeletedAt == null)))
            .ForMember(dest => dest.UserReactFlag, opt => opt
                .MapFrom(src => src.Reacts
                    .Where(r => r.UserId == currentUserId)
                    .Select(r => r.ReactFlag)
                    .SingleOrDefault()));

        CreateMap<PostEditDTO, Post>()
            .ForMember(dest => dest.Images, opt => opt.Ignore());
        CreateMap<PostEditDTO, UserPost>()
            .ForMember(dest => dest.Images, opt => opt.Ignore());
        CreateMap<PostEditDTO, GroupPost>()
            .ForMember(dest => dest.Images, opt => opt.Ignore());
    }
}
