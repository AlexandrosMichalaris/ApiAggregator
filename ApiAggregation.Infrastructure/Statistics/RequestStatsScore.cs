using System.Collections.Concurrent;
using ApiAggregation.Application.Interface;
using ApiAggregation.Model.Statistics;

namespace ApiAggregation.Application.Service;

public class RequestStatsStore : IRequestStatsStore
{
    private readonly ConcurrentDictionary<string, List<long>> _requestDurations = new();

    public void Add(string serviceName, long duration)
    {
        var durations = _requestDurations.GetOrAdd(serviceName, _ => new List<long>());
        lock (durations)
        {
            durations.Add(duration);
        }
    }

    public IEnumerable<ApiStatisticsDto> GetAllStats()
    {
        foreach (var entry in _requestDurations)
        {
            var durations = entry.Value.ToArray(); // Copy to avoid threading issues
            var count = durations.Length;
            var average = durations.Any() ? durations.Average() : 0;

            yield return new ApiStatisticsDto
            {
                ApiName = entry.Key,
                TotalRequests = count,
                AverageResponseTime = average,
                PerformanceBucket = Bucket(average)
            };
        }
    }

    private string Bucket(double avg)
    {
        if (avg < 300) return "Fast";
        if (avg < 500) return "Average";
        return "Slow";
    }
}
