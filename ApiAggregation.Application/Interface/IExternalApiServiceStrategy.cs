namespace ApiAggregation.Application.Interfaces;

public interface IExternalApiServiceStrategy
{
    IExternalApiService<TRequest, TResponse> GetExternalApiService<TRequest, TResponse>(String Name)
        where TRequest : class 
        where TResponse : class;
}