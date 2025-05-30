using ApiAggregation.Application.Interfaces;

namespace ApiAggregation.Application.Strategy;

public class ExternalApiServiceStrategy : IExternalApiServiceStrategy
{
    //TRequest and TResponse are not known at compile time, so we add "object" for the DI.
    private readonly IEnumerable<object> _externalApiService;

    public ExternalApiServiceStrategy(IEnumerable<object> externalApiService)
    {
        _externalApiService = externalApiService;
    }
    
    
    public IExternalApiService<TRequest, TResponse> GetExternalApiService<TRequest, TResponse>(string name) 
        where TRequest : class 
        where TResponse : class
    {
        var typedServices = _externalApiService
            .OfType<IExternalApiService<TRequest, TResponse>>();
        
        var service = typedServices.SingleOrDefault(s => s.Name == name);
        
        if(service is null)
            throw new ArgumentException($"No external API service with name {name} found.");
        
        return service;
    }
}