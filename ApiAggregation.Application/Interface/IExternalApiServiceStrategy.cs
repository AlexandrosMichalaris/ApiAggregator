namespace ApiAggregation.Application.Interfaces;

public interface IExternalApiServiceStrategy
{
    Task<IExternalApiService<TRequest, TResponse>> GetExternalApiService<TRequest, TResponse>()
        where TRequest : class 
        where TResponse : class;
}