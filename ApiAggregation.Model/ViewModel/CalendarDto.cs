namespace ApiAggregation.Model.Dto;

public class CalendarDto
{
    public IEnumerable<CalendarDataDto> Data { get; set; }
}

public class CalendarDataDto
{
    public string Fixed  { get; set; }
    
    public string LocalName { get; set; }
    
    public string Name { get; set; }
    
    public string Global { get; set; }
    
    public string Timestamp { get; set; }
}
