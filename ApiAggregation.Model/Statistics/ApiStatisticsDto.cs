namespace ApiAggregation.Model.Statistics;

// File: Application/Models/ApiStatisticsDto.cs
public class ApiStatisticsDto
{
    public string ApiName { get; set; } = string.Empty;
    
    public int TotalRequests { get; set; }
    
    public double AverageResponseTimeMs { get; set; }
    
    public string PerformanceCategory { get; set; } = string.Empty;
}
