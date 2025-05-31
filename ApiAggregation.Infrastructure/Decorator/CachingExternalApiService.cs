using System.Text.Json;
using ApiAggregation.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ApiAggragation.Infrastructure.Decorator;

public class ExternalApiServiceCachingDecorator<TRequest, TResponse> 
    : IExternalApiService<TRequest, TResponse>
    where TRequest : class 
    where TResponse : class
{
    private readonly IExternalApiService<TRequest, TResponse> _inner;
    private readonly IMemoryCache _cache;
    private readonly ILogger _logger;

    public ExternalApiServiceCachingDecorator(
        IExternalApiService<TRequest, TResponse> inner,
        IMemoryCache cache,
        ILogger<ExternalApiServiceCachingDecorator<TRequest, TResponse>> logger)
    {
        _inner = inner;
        _cache = cache;
        _logger = logger;
    }

    public string Name => _inner.Name;

    public async Task<TResponse> FetchAsync(TRequest request)
    {
        var cacheKey = $"{typeof(TRequest).Name}_{JsonSerializer.Serialize(request)}";

        if (_cache.TryGetValue(cacheKey, out TResponse cached))
        {
            _logger.LogInformation("Cache hit");
            return cached;
        }

        var result = await _inner.FetchAsync(request);
        _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }
}
