namespace ApiAggregation.Model.Request;

public class NewsApiRequest
{
    public string Query { get; set; }
    
    public string Mode { get; set; } = "artlist";
    
    public string Format { get; set; } = "json";

    public string ToQueryString()
    {
        // Encodes characters in the input string so they are URL-safe
        return $"query={Uri.EscapeDataString(Query)}&mode={Mode}&format={Format}";
    }
}