using AutoMapper;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.Services.Mapping;

public class RequestProfile : Profile
{
    public RequestProfile()
    {
        CreateMap<JoinGroupRequest, JoinGroupRequestDTO>();
        CreateMap<PromotionRequest, PromotionRequestDTO>();
        CreateMap<MangaRequest, MangaRequestDTO>();
        CreateMap<OtherRequest, OtherRequestDTO>();

        CreateMap<PromotionRequestEditDTO, PromotionRequest>();
        CreateMap<MangaRequestEditDTO, MangaRequest>();
        CreateMap<OtherRequestEditDTO, OtherRequest>();
    }
}
