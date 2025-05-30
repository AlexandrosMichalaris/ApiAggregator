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

public class OpenWeatherApiService : IExternalApiService<OpenWeatherApiRequest, WeatherDto>
{
    private readonly HttpClient _httpClient;
    private readonly OpenWeatherSettings _settings;
    private readonly ILogger<OpenWeatherApiService> _logger;
    private readonly IMapper _mapper;
    
    public string Name => "OpenWeather";
    
    #region ctor

    public OpenWeatherApiService(
        HttpClient httpClient, 
        IOptions<OpenWeatherSettings> options,
        ILogger<OpenWeatherApiService> logger,
        IMapper mapper)
    {
        _httpClient = httpClient;
        _settings = options.Value;
        _logger = logger;
        _mapper = mapper;
    }

    #endregion

    public async Task<WeatherDto> FetchAsync(OpenWeatherApiRequest request)
    {
        try
        {
            var url = $"{_settings.BaseUrl}?{request.ToQueryString()}";

            var httpResponse = await _httpClient.GetAsync(url);

            if (!httpResponse.IsSuccessStatusCode)
            {
                _logger.LogError($"Open-Meteo API returned a non-success status code: {httpResponse.StatusCode}");
                throw new HttpRequestException(
                    $"Open-Meteo API returned a non-success status code: {httpResponse.StatusCode}");
            }

            var response = await httpResponse.Content.ReadFromJsonAsync<OpenWeatherApiResponse>();

            if (response is null)
            {
                _logger.LogError("Open-Meteo response deserialization returned null.");
                throw new InvalidOperationException("Failed to deserialize Open-Meteo response.");
            }

            return _mapper.Map<WeatherDto>(response);
        }
        // catch (HttpRequestException ex)
        // {
        //     //TODO
        // }
        catch (Exception e)
        {
            _logger.LogError(e, $"Failed to fetch or process Open-Meteo data. Message: {e.Message}");
            throw;
        }
    }
}