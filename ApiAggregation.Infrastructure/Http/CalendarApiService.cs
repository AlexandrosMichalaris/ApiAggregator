using System.Net.Http.Json;
using ApiAggragation.Infrastructure.Configuration;
using ApiAggregation.Application.Interfaces;
using ApiAggregation.Model.Constants;
using ApiAggregation.Model.Dto;
using ApiAggregation.Model.Dto.Response;
using ApiAggregation.Model.Request;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApiAggragation.Infrastructure.Http;

public class CalendarApiService : IExternalApiService<CalendarApiRequest, CalendarDto>
{
    private readonly HttpClient _httpClient;
    private readonly CalendarSettings _settings;
    private readonly ILogger<CalendarApiService> _logger;
    private readonly IMapper _mapper;

    #region Ctor

    public CalendarApiService(
        HttpClient httpClient,
        IOptions<CalendarSettings> options,
        ILogger<CalendarApiService> logger,
        IMapper mapper)
    {
        _httpClient = httpClient;
        _settings = options.Value;
        _logger = logger;
        _mapper = mapper;
    }

    #endregion
    
    public string Name => Constants.CalendarName;
    
    public async Task<CalendarDto> FetchAsync(CalendarApiRequest request)
    {
        try
        {
            var url = $"{_settings.BaseUrl}{request.ToQueryString()}";
        
            var httpResponse = await _httpClient.GetAsync(url);
        
            if (!httpResponse.IsSuccessStatusCode)
            {
                _logger.LogError($"Calendar API returned a non-success status code: {httpResponse.StatusCode}");
                throw new HttpRequestException(
                    $"Calendar API returned a non-success status code: {httpResponse.StatusCode}");
            }

            //var response = await httpResponse.Content.ReadFromJsonAsync<CalendarApiResponse>();
            
            var holidays = await httpResponse.Content.ReadFromJsonAsync<List<CalendarHolidayApiResponse>>();
            
            var response = new CalendarApiResponse
            {
                Holidays = holidays
            };

            if (response is null)
            {
                _logger.LogError("Calendar response deserialization returned null.");
                throw new InvalidOperationException("Failed to deserialize Calendar response.");
            }
            
            var dto = _mapper.Map<CalendarDto>(response);
            
            return dto;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Failed to fetch or process Calendar data. Message: {e.Message}");
            throw;
        }
    }
    
}