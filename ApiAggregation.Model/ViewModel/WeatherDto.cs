namespace ApiAggregation.Model.Dto;

public class WeatherDto
{
    public double Temperature { get; set; }
    
    public string Timezone { get; set; }
    
    public string Time { get; set; }
    
    public int WeatherCode { get; set; }
    
    public bool Success { get; set; } = false;
}
