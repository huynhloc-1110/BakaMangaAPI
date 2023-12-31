﻿using AutoMapper;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.Services.Mapping;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        string? currentUserId = null;

        CreateMap<Comment, CommentDTO>()
            .ForMember(dest => dest.ChildCommentCount, opt => opt
                .MapFrom(src => src.ChildComments.Count(c => c.DeletedAt == null)))
            .ForMember(dest => dest.LikeCount, opt => opt
                .MapFrom(src => src.Reacts.Count(r => r.ReactFlag == ReactFlag.Like)))
            .ForMember(dest => dest.DislikeCount, opt => opt
                .MapFrom(src => src.Reacts.Count(r => r.ReactFlag == ReactFlag.Dislike)))
            .ForMember(dest => dest.UserReactFlag, opt => opt
                .MapFrom(src => src.Reacts
                    .Where(r => r.UserId == currentUserId)
                    .Select(r => r.ReactFlag)
                    .SingleOrDefault()));

        CreateMap<CommentEditDTO, Comment>();
        CreateMap<CommentEditDTO, MangaComment>();
        CreateMap<CommentEditDTO, ChapterComment>();
        CreateMap<CommentEditDTO, PostComment>();
    }
}
