namespace ApiAggregation.Model.Request;

public class WeatherApiRequest
{
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
    
    public string Current { get; set; } = "temperature_2m,weather_code";
    
    public int Forecast_Days { get; set; } = 1;

    public string ToQueryString()
    {
        return $"latitude={Latitude}&longitude={Longitude}&current={Current}&forecast_days={Forecast_Days}";
    }
}