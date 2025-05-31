namespace ApiAggregation.Model.Dto;

public class CalendarDto
{
    public IEnumerable<HolidayData>? Holidays { get; set; }
}

public class HolidayData
{
    public bool Fixed  { get; set; }
    
    public string LocalName { get; set; }
    
    public string Name { get; set; }
    
    public string Global { get; set; }
    
    public DateTime Date { get; set; }
}
