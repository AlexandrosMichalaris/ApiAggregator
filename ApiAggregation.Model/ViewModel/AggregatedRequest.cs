namespace ApiAggregation.Model.Dto;

public class AggregatedRequest
{
    public int Year { get; set; }
    
    public string CountryCode { get; set; }
    
    public string Query { get; set; }
    
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
    
    public bool? Sort { get; set; }
    
    public DateTime? FilterDate { get; set; }
    
    public string? CountryArticle { get; set; }
}