using ApiAggragation.Infrastructure.Decorator;
using ApiAggragation.Infrastructure.Http;
using ApiAggregation.Application.Interfaces;
using ApiAggregation.Application.Strategy;
using ApiAggregation.Mapping;
using ApiAggregation.Model.Dto;
using ApiAggregation.Model.Request;
using Microsoft.Extensions.Caching.Memory;

namespace ApiAggregation.Configuration.DI;

public static class DiConfiguration
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        // Request services
        services.AddScoped<IExternalApiService<NewsApiRequest, NewsArticleDto>, NewsApiService>();
        services.AddScoped<IExternalApiService<CalendarApiRequest, CalendarDto>, CalendarApiService>();
        
        /**
         * IExternalApiService is stateless
         * IMemoryCache is shared and injected
         * Transient New instance per injection
         */
        
        // New instance per injection
        services.AddTransient<OpenWeatherApiService>();
        services.AddTransient<NewsApiService>();
        services.AddTransient<CalendarApiService>();

        // Register caching decorators
        services.AddTransient<IExternalApiService<OpenWeatherApiRequest, WeatherDto>>(sp =>
            new ExternalApiServiceCachingDecorator<OpenWeatherApiRequest, WeatherDto>(
                sp.GetRequiredService<OpenWeatherApiService>(),
                sp.GetRequiredService<IMemoryCache>(),
                sp.GetRequiredService<ILogger<ExternalApiServiceCachingDecorator<OpenWeatherApiRequest, WeatherDto>>>()
            ));

        services.AddTransient<IExternalApiService<NewsApiRequest, NewsArticleDto>>(sp =>
            new ExternalApiServiceCachingDecorator<NewsApiRequest, NewsArticleDto>(
                sp.GetRequiredService<NewsApiService>(),
                sp.GetRequiredService<IMemoryCache>(),
                sp.GetRequiredService<ILogger<ExternalApiServiceCachingDecorator<NewsApiRequest, NewsArticleDto>>>()
            ));
        
        services.AddTransient<IExternalApiService<CalendarApiRequest, CalendarDto>>(sp =>
            new ExternalApiServiceCachingDecorator<CalendarApiRequest, CalendarDto>(
                sp.GetRequiredService<CalendarApiService>(),
                sp.GetRequiredService<IMemoryCache>(),
                sp.GetRequiredService<ILogger<ExternalApiServiceCachingDecorator<CalendarApiRequest, CalendarDto>>>()
            ));
        
        services.AddSingleton<IExternalApiServiceStrategy, ExternalApiServiceStrategy>();
        
        
        services.AddHttpClient<OpenWeatherApiService>();
        services.AddHttpClient<CalendarApiService>();
        services.AddHttpClient<NewsApiService>();
        
        
        services.AddAutoMapper(typeof(WeatherMappingProfile));
        services.AddAutoMapper(typeof(CalendarMappingProfile));
        services.AddAutoMapper(typeof(NewsMappingProfile));
        
        
        
        

        
        // services.AddAutoMapper(typeof(FileRecordProfile)); // points to any profile in that assembly
        // services.AddAutoMapper(typeof(JobFileRecordProfile)); // points to any profile in that assembly
        // services.AddAutoMapper(typeof(LoginAttemptProfile));
        // services.AddAutoMapper(typeof(TrustedIpProfile));
    }
}