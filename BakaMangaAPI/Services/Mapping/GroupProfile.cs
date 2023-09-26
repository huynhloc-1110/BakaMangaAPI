using AutoMapper;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.Services.Mapping;

public class GroupProfile : Profile
{
    public GroupProfile()
    {
        CreateMap<Group, GroupBasicDTO>()
            .ForMember(dest => dest.MemberNumber, opt => opt
                .MapFrom(src => src.Members.Count));

        CreateMap<Group, GroupDetailDTO>()
            .ForMember(dest => dest.MemberNumber, opt => opt
                .MapFrom(src => src.Members.Count))
            .ForMember(dest => dest.UploadedChapterNumber, opt => opt
                .MapFrom(src => src.Chapters.Count))
            .ForMember(dest => dest.ViewGainedNumber, opt => opt
                .MapFrom(src => src.Chapters.Sum(c => c.ChapterViews.Count)));

        CreateMap<GroupEditDTO, Group>();

        CreateMap<GroupMember, GroupMemberDTO>()
            .ForMember(dest => dest.Id, opt => opt
                .MapFrom(src => src.UserId))
            .ForMember(dest => dest.Name, opt => opt
                .MapFrom(src => src.User.Name))
            .ForMember(dest => dest.AvatarPath, opt => opt
                .MapFrom(src => src.User.AvatarPath));
    }
}
