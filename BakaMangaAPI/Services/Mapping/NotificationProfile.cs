using AutoMapper;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.Services.Mapping;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<RequestNotification, RequestNotificationDTO>()
            .ForMember(dest => dest.RequestType, opt => opt
                .MapFrom(src => src.Request.GetType().Name));
        CreateMap<ChapterNotification, ChapterNotificationDTO>();
        CreateMap<GroupNotification, GroupNotificationDTO>();
        CreateMap<FollowerNotification, FollowerNotificationDTO>();
    }
}
