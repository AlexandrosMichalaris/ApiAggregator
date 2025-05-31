using ApiAggregation.Application.Interfaces;
using ApiAggregation.Interface;
using ApiAggregation.Model.Constants;
using ApiAggregation.Model.Dto;
using ApiAggregation.Model.Request;
using Microsoft.Extensions.Logging;

namespace ApiAggregation.Application.Service;

public class AggregatedApiService : IAggregatedApiService
{
    private readonly IExternalApiServiceStrategy _strategy;
    private readonly ILogger<AggregatedApiService> _logger;
    
    
    public async Task<AggregatedResult> GetAggregatedResultsAsync(AggregatedRequest request)
    {
        var weatherService = await _strategy.GetExternalApiService<OpenWeatherApiRequest, WeatherDto>(Constants.WeatherName);
        var newsService = await _strategy.GetExternalApiService<NewsApiRequest, NewsArticleDto>(Constants.NewsName);
        var calendarService = await _strategy.GetExternalApiService<CalendarApiRequest, CalendarDto>(Constants.CalendarName);
        
        var weatherTask = SafeExecuteAsync(() => weatherService.FetchAsync(new OpenWeatherApiRequest()
        {
            Latitude = request.Latitude,
            Longitude = request.Longitude,
        }), Constants.WeatherName);
        
        var newsTask = SafeExecuteAsync(() => newsService.FetchAsync(new NewsApiRequest()
        {
            Query = request.Query
        }), Constants.NewsName);
        
        var calendarTask = SafeExecuteAsync(() => calendarService.FetchAsync(new CalendarApiRequest()
        {
            Year = request.Year,
            CountryCode = request.CountryCode
        }), Constants.CalendarName);
        
        await Task.WhenAll(weatherTask, newsTask, calendarTask);

        return new AggregatedResult()
        {
            Weather = weatherTask.Result,
            News = newsTask.Result,
            Calendar = calendarTask.Result,
            Timestamp = DateTimeOffset.Now
        };
    }

    /// <summary>
    /// Wrapper to log error in the parallel excecution 
    /// </summary>
    /// <param name="action"></param>
    /// <param name="ApiName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private async Task<T> SafeExecuteAsync<T>(Func<Task<T>> action, string ApiName)
    {
        try
        {
            return await action();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error fetching the API {ApiName}");
            throw;
        }
    }
    
}