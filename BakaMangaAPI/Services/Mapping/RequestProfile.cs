using AutoMapper;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.Services.Mapping;

public class RequestProfile : Profile
{
    public RequestProfile()
    {
        CreateMap<JoinGroupRequest, JoinGroupRequestDTO>();
    }
}
