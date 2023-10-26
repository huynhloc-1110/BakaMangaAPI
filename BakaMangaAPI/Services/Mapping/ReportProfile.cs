using AutoMapper;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.Services.Mapping;

public class ReportProfile : Profile
{
    public ReportProfile()
    {
        CreateMap<UserReportEditDTO, UserReport>();
        CreateMap<UserReport, UserReportDTO>();
    }
}