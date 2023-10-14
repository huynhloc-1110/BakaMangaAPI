using AutoMapper;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.Services.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ApplicationUser, UserBasicDTO>()
            .ForMember(dest => dest.Roles, opt => opt
                .MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)));

        CreateMap<ApplicationUser, UserSimpleDTO>();

        CreateMap<ApplicationUserFollow, UserFollowDTO>();
    }
}
