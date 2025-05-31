namespace ApiAggregation.Application.Interfaces;

public interface IExternalApiServiceStrategy
{
    Task<IExternalApiService<TRequest, TResponse>> GetExternalApiService<TRequest, TResponse>(string Name)
        where TRequest : class 
        where TResponse : class;
}