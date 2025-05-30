using ApiAggregation.Model.Dto;

namespace ApiAggregation.Application.Interfaces;

public interface IExternalApiService<TRequest, TResponse> 
    where TRequest : class 
    where TResponse : class
{
    string Name { get; }

    Task<TResponse> FetchAsync(TRequest request);
}