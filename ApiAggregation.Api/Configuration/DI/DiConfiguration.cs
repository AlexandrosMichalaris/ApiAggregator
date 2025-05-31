using ApiAggragation.Infrastructure.Decorator;
using ApiAggragation.Infrastructure.Http;
using ApiAggregation.Application.Interfaces;
using ApiAggregation.Application.Service;
using ApiAggregation.Application.Strategy;
using ApiAggregation.Interface;
using ApiAggregation.Mapping;
using ApiAggregation.Model.Dto;
using ApiAggregation.Model.Request;
using Microsoft.Extensions.Caching.Memory;

namespace ApiAggregation.Configuration.DI;

public static class DiConfiguration
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        // New instance per injection
        services.AddScoped<WeatherApiService>();
        services.AddScoped<NewsApiService>();
        services.AddScoped<CalendarApiService>();

        // Register caching decorators
        services.AddScoped<IExternalApiService<WeatherApiRequest, WeatherDto>>(sp =>
            new ExternalApiServiceCachingDecorator<WeatherApiRequest, WeatherDto>(
                sp.GetRequiredService<WeatherApiService>(),
                sp.GetRequiredService<IMemoryCache>(),
                sp.GetRequiredService<ILogger<ExternalApiServiceCachingDecorator<WeatherApiRequest, WeatherDto>>>()
            ));

        services.AddScoped<IExternalApiService<NewsApiRequest, NewsArticleDto>>(sp =>
            new ExternalApiServiceCachingDecorator<NewsApiRequest, NewsArticleDto>(
                sp.GetRequiredService<NewsApiService>(),
                sp.GetRequiredService<IMemoryCache>(),
                sp.GetRequiredService<ILogger<ExternalApiServiceCachingDecorator<NewsApiRequest, NewsArticleDto>>>()
            ));
        
        services.AddScoped<IExternalApiService<CalendarApiRequest, CalendarDto>>(sp =>
            new ExternalApiServiceCachingDecorator<CalendarApiRequest, CalendarDto>(
                sp.GetRequiredService<CalendarApiService>(),
                sp.GetRequiredService<IMemoryCache>(),
                sp.GetRequiredService<ILogger<ExternalApiServiceCachingDecorator<CalendarApiRequest, CalendarDto>>>()
            ));
        
        services.AddSingleton<IExternalApiServiceStrategy, ExternalApiServiceStrategy>();
        
        services.AddScoped<IAggregatedApiService, AggregatedApiService>();
        
        
        services.AddHttpClient<WeatherApiService>();
        services.AddHttpClient<CalendarApiService>();
        services.AddHttpClient<NewsApiService>();
        
        
        services.AddAutoMapper(typeof(WeatherMappingProfile));
        services.AddAutoMapper(typeof(CalendarMappingProfile));
        services.AddAutoMapper(typeof(NewsMappingProfile));
        
        
        services.AddMemoryCache();
    }
}