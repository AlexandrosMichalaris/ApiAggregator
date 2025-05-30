using System.Net.Http.Json;
using ApiAggragation.Infrastructure.Configuration;
using ApiAggregation.Application.Interfaces;
using ApiAggregation.Model.Dto;
using ApiAggregation.Model.Dto.Response;
using ApiAggregation.Model.Request;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApiAggragation.Infrastructure.Http;

public class NewsApiService : IExternalApiService<NewsApiRequest, NewsArticleDto>
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly NewsSettings _settings;
    private readonly ILogger<NewsApiService> _logger;

    #region Ctor

    public NewsApiService(
        HttpClient httpClient,
        IOptions<NewsSettings> settings,
        ILogger<NewsApiService> logger,
        IMapper mapper)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;
        _mapper = mapper;
    }

    #endregion
    
    public string Name => "News";
    public async Task<NewsArticleDto> FetchAsync(NewsApiRequest request)
    {
        try
        {
            var url = $"{_settings.BaseUrl}?{request.ToQueryString()}";
            
            var httpResponse = await _httpClient.GetAsync(url);

            if (!httpResponse.IsSuccessStatusCode)
            {
                _logger.LogError($"News API returned a non-success status code: {httpResponse.StatusCode}");
                throw new HttpRequestException(
                    $"News API returned a non-success status code: {httpResponse.StatusCode}");
            }

            var response = await httpResponse.Content.ReadFromJsonAsync<NewsApiResponse>();

            if (response is null)
            {
                _logger.LogError("News response deserialization returned null.");
                throw new InvalidOperationException("Failed to deserialize News response.");
            }
            
            return _mapper.Map<NewsArticleDto>(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Failed to fetch or process News data. Message: {e.Message}");
            throw;
        }
    }

}