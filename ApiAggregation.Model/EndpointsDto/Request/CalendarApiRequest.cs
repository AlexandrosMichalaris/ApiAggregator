namespace ApiAggregation.Model.Request;

public class CalendarApiRequest
{
    public int Year { get; set; }
    
    public string CountryCode { get; set; } = "GR";

    public string ToQueryString()
    {
        return $"/{Year}/{CountryCode}";
    }
}