namespace ApiAggregation.Model.Dto;

public class AggregatedResult
{
    //public LocationDto Location { get; set; }
    public WeatherDto Weather { get; set; }
    
    public CalendarDto Calendar { get; set; }
    
    public NewsArticleDto News { get; set; }
    
    public DateTime Timestamp { get; set; }
}