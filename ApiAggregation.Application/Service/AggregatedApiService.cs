using System.Text.Json;
using ApiAggregation.Application.Helper;
using ApiAggregation.Application.Interfaces;
using ApiAggregation.Interface;
using ApiAggregation.Model.Constants;
using ApiAggregation.Model.Dto;
using ApiAggregation.Model.Request;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ApiAggregation.Application.Service;

public class AggregatedApiService : IAggregatedApiService
{
    private readonly IExternalApiServiceStrategy _strategy;
    private readonly ILogger<AggregatedApiService> _logger;
    private readonly IMemoryCache _cache;

    #region Ctor

    public AggregatedApiService(
        IExternalApiServiceStrategy strategy,
        ILogger<AggregatedApiService> logger,
        IMemoryCache cache)
    {
        _strategy = strategy;
        _logger = logger;
        _cache = cache;
    }

    #endregion
    
    public async Task<AggregatedResult> GetAggregatedResultsAsync(AggregatedRequest request)
    {
        var weatherService = await _strategy.GetExternalApiService<WeatherApiRequest, WeatherDto>();
        var newsService = await _strategy.GetExternalApiService<NewsApiRequest, NewsArticleDto>();
        var calendarService = await _strategy.GetExternalApiService<CalendarApiRequest, CalendarDto>();
        
        var weatherRequest = new WeatherApiRequest
        {
            Latitude = request.Latitude,
            Longitude = request.Longitude,
        };

        var newsRequest = new NewsApiRequest
        {
            Query = request.Query,
            Sort = request.Sort,
            CountryArticle = request.CountryArticle
        };

        var calendarRequest = new CalendarApiRequest
        {
            Year = request.Year,
            CountryCode = request.CountryCode,
            FilterDate = request.FilterDate,
        };
        
        var weatherTask = SafeExecuteAsync<WeatherApiRequest, WeatherDto>(
            () => weatherService.FetchAsync(weatherRequest),
            weatherRequest,
            Constants.WeatherName,
            _cache);

        var newsTask = SafeExecuteAsync<NewsApiRequest, NewsArticleDto>(
            () => newsService.FetchAsync(newsRequest),
            newsRequest,
            Constants.NewsName,
            _cache);

        var calendarTask = SafeExecuteAsync<CalendarApiRequest, CalendarDto>(
            () => calendarService.FetchAsync(calendarRequest),
            calendarRequest,
            Constants.CalendarName,
            _cache);
        
        await Task.WhenAll(weatherTask, newsTask, calendarTask);

        // Apply filtering from helper
        return AggregationHelper.ApplyFilteringAndSorting(new AggregatedResult()
        {
            Weather = weatherTask.Result,
            News = newsTask.Result,
            Calendar = calendarTask.Result,
            Timestamp = DateTimeOffset.Now
        }, request);
    }

    /// <summary>
    /// Wrapper to log error in the parallel excecution.
    /// And fallback mechanism, if something goes wrong,
    /// Returns cashed values, or empty response.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="request"></param>
    /// <param name="apiName"></param>
    /// <param name="cache"></param>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public async Task<TResponse> SafeExecuteAsync<TRequest, TResponse>(
        Func<Task<TResponse>> action,
        TRequest request,
        string apiName,
        IMemoryCache cache)
        where TResponse : class, new()
    {
        try
        {
            return await action();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error fetching the API {apiName}");

            var cacheKey = $"{typeof(TRequest).Name}_{JsonSerializer.Serialize(request)}";

            if (cache.TryGetValue(cacheKey, out TResponse cached))
            {
                _logger.LogWarning($"Using cached response for {apiName}");
                return cached;
            }

            _logger.LogWarning($"Returning empty object for {apiName}");
            return new TResponse();
        }
    }
}