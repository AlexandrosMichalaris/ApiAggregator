namespace ApiAggregation.Model.Dto.Response;

public class NewsApiResponse
{
    public List<GdeltArticle> Articles { get; set; }
}

public class GdeltArticle
{
    public string Url { get; set; }
    
    public string Title { get; set; }
    
    public string Seendate { get; set; }
    
    public string Socialimage { get; set; }
    
    public string Sourcecountry { get; set; }
    
    public string Source { get; set; }
    
    public string Domain { get; set; }
    
    public string Language { get; set; }
    
    public DateTime Datetime { get; set; }
}