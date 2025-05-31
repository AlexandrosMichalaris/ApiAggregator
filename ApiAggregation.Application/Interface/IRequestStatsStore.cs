using ApiAggregation.Model.Statistics;

namespace ApiAggregation.Application.Interface;

public interface IRequestStatsStore
{
    void Add(string serviceName, long duration);
    
    IEnumerable<ApiStatisticsDto> GetAllStats();
}