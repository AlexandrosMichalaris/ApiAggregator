namespace ApiAggregation.Model.Dto;

public class NewsArticleDto
{
    public IEnumerable<Article> Articles { get; set; }
}

public class Article
{
    public string Title { get; set; }
    
    public string Url { get; set; }
    
    public string Source { get; set; }
    
    public string Language { get; set; }
    
    public DateTime PublishedAt { get; set; }
}