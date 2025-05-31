using ApiAggregation.Model.Dto;

namespace ApiAggregation.Interface;

public interface IAggregatedApiService
{
    Task<AggregatedResult> GetAggregatedResultsAsync(AggregatedRequest request);
}