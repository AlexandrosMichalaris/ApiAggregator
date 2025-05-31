using ApiAggregation.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ApiAggregation.Application.Strategy;

public class ExternalApiServiceStrategy : IExternalApiServiceStrategy
{
    private readonly IServiceProvider _serviceProvider;

    public ExternalApiServiceStrategy(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    
    public Task<IExternalApiService<TRequest, TResponse>> GetExternalApiService<TRequest, TResponse>() 
        where TRequest : class 
        where TResponse : class
    {
        var service = _serviceProvider.GetService<IExternalApiService<TRequest, TResponse>>();
        
        if(service is null)
            throw new ArgumentException($"No external API service found for {typeof(TRequest).Name} → {typeof(TResponse).Name}");
        
        return Task.FromResult(service);
    }
}