namespace ApiAggregation.Model.Dto;

public class AggregatedRequest
{
    public int Year { get; set; }
    
    public string CountryCode { get; set; }
    
    public string Query { get; set; }
    
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
}