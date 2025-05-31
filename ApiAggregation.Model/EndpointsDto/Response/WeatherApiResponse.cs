namespace ApiAggregation.Model.Dto.Response;

public class WeatherApiResponse
{
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
    
    public double GenerationtimeMs { get; set; }
    
    public int UtcOffsetSeconds { get; set; }
    
    public string Timezone { get; set; }
    
    public string TimezoneAbbreviation { get; set; }
    
    public double Elevation { get; set; }
    
    public CurrentWeather Current { get; set; }
}

public class CurrentWeather
{
    public DateTime Time { get; set; }
    
    public int Interval { get; set; }
    
    public double Temperature_2m { get; set; }
    
    public int Weather_Code { get; set; }
}