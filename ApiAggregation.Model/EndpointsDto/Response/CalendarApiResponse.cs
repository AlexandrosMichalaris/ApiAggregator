namespace ApiAggregation.Model.Dto.Response;

public class CalendarApiResponse
{
    public List<CalendarData> Data { get; set; }
}

public class CalendarData
{
    public string Date { get; set; }
    
    public string LocalName { get; set; }
    
    public string Name { get; set; }
    
    public string CountryCode { get; set; }
    
    public string Fixed  { get; set; }
    
    public string Global  { get; set; }
    
    public string? LaunchYear { get; set; }
    
    public IEnumerable<string> types { get; set; }
}

