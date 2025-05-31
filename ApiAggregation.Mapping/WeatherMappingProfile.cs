using ApiAggregation.Model.Dto;
using ApiAggregation.Model.Dto.Response;
using AutoMapper;

namespace ApiAggregation.Mapping;

public class WeatherMappingProfile : Profile
{
    public WeatherMappingProfile()
    {
        CreateMap<WeatherApiResponse, WeatherDto>()
            .ForMember(dest => dest.Temperature, opt => opt.MapFrom(src => src.Current.Temperature_2m))
            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Current.Time))
            .ForMember(dest => dest.WeatherCode, opt => opt.MapFrom(src => src.Current.Weather_Code))
            .ForMember(dest => dest.Timezone, opt => opt.MapFrom(src => src.Timezone));
    }
}