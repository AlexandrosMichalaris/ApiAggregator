namespace ApiAggregation.Model.Statistics;

// File: Application/Models/ApiStatisticsDto.cs
public class ApiStatisticsDto
{
    public string ApiName { get; set; } = string.Empty;
    
    public int TotalRequests { get; set; }
    
    public double AverageResponseTime { get; set; }
    
    public string PerformanceBucket { get; set; } = string.Empty;
}
