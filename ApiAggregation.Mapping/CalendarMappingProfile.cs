using ApiAggregation.Model.Dto;
using ApiAggregation.Model.Dto.Response;
using AutoMapper;

namespace ApiAggregation.Mapping;

public class CalendarMappingProfile : Profile
{
    public CalendarMappingProfile()
    {
        CreateMap<CalendarData, CalendarDataDto>()
            .ForMember(dest => dest.Fixed, opt => opt.MapFrom(src => src.Fixed))
            .ForMember(dest => dest.Global, opt => opt.MapFrom(src => src.Global))
            .ForMember(dest => dest.LocalName, opt => opt.MapFrom(src => src.LocalName))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Date));

        CreateMap<CalendarApiResponse, CalendarDto>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));
    }
}