namespace ApiAggregation.Model.Request;

public class CalendarApiRequest
{
    public int Year { get; set; }
    
    public string CountryCode { get; set; } = "GR";
    
    // Filtering value (not sent in the request, used for caching)
    public DateTime? FilterDate { get; set; }

    public string ToQueryString()
    {
        return $"/{Year}/{CountryCode}";
    }
}