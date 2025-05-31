namespace ApiAggregation.Model.Dto.Response;

public class CalendarApiResponse
{
    public List<CalendarHolidayApiResponse> Holidays { get; set;  }
}

public class CalendarHolidayApiResponse
{
    public DateTime Date { get; set; }
    
    public string LocalName { get; set; }
    
    public string Name { get; set; }
    
    public bool Fixed  { get; set; }
    
    public bool Global  { get; set; }
    
    //public List<string> types { get; set; }
}
