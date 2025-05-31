using System.Diagnostics;
using ApiAggregation.Application.Interface;
using ApiAggregation.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace ApiAggragation.Infrastructure.Decorator;

public class ExternalApiServiceStatsDecorator<TRequest, TResponse> : IExternalApiService<TRequest, TResponse>
    where TRequest : class where TResponse : class
{
    private readonly IExternalApiService<TRequest, TResponse> _inner;
    private readonly IRequestStatsStore _statsStore;
    private readonly ILogger _logger;

    public ExternalApiServiceStatsDecorator(
        IExternalApiService<TRequest, TResponse> inner,
        IRequestStatsStore statsStore,
        ILogger<ExternalApiServiceStatsDecorator<TRequest, TResponse>> logger)
    {
        _inner = inner;
        _statsStore = statsStore;
        _logger = logger;
    }

    public string Name => _inner.Name;

    public async Task<TResponse> FetchAsync(TRequest request)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var result = await _inner.FetchAsync(request);
            return result;
        }
        finally
        {
            stopwatch.Stop();
            _statsStore.Add(Name, stopwatch.ElapsedMilliseconds);
            _logger.LogInformation("Recorded request duration for {Name}: {Elapsed}ms", Name, stopwatch.ElapsedMilliseconds);
        }
    }
}