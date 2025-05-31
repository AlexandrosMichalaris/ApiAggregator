using ApiAggregation.Model.Dto;
using ApiAggregation.Model.Dto.Response;
using AutoMapper;

namespace ApiAggregation.Mapping;

public class NewsMappingProfile : Profile
{
    public NewsMappingProfile()
    {
        CreateMap<GdeltArticle, Article>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
            .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source))
            .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language))
            .ForMember(dest => dest.Sourcecountry, opt => opt.MapFrom(src => src.Sourcecountry));
            //.ForMember(dest => dest.PublishedAt, opt => opt.MapFrom(src => src.Datetime));

        CreateMap<NewsApiResponse, NewsArticleDto>()
            .ForMember(dest => dest.Articles, opt => opt.MapFrom(src => src.Articles));
    }
}