namespace ApiAggregation.Model.Request;

public class NewsApiRequest
{
    public string Query { get; set; }
    
    public string Mode { get; set; } = "artlist";
    
    public string Format { get; set; } = "json";
    
    // Filtering value (not sent in the request)
    public bool? Sort { get; set; }
    
    public string? CountryArticle { get; set; }

    public string ToQueryString()
    {
        // Encodes characters in the input string so they are URL-safe
        return $"query={Uri.EscapeDataString(Query)}&mode={Mode}&format={Format}";
    }
}