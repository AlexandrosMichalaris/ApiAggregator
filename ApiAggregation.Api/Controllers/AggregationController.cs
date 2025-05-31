using ApiAggregation.Interface;
using ApiAggregation.Model.Dto;
using Microsoft.AspNetCore.Mvc;
using Model.ApiResponse;

namespace ApiAggregation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AggregationController : ControllerBase
{
    private readonly IAggregatedApiService _aggregatedApiService;
    private readonly ILogger<AggregationController> _logger;

    #region Ctor

    public AggregationController(
        IAggregatedApiService aggregatedApiService,
        ILogger<AggregationController> logger)
    {
        _aggregatedApiService = aggregatedApiService;
        _logger = logger;
    }

    #endregion

    [HttpGet("get")]
    public async Task<ActionResult<ApiResponse<AggregatedResult>>> GetAggregatedResultAsync([FromQuery] AggregatedRequest request)
    {
        var validationError = ValidateRequest(request);
        if (validationError is not null)
            return BadRequest(new ApiResponse<AggregatedResult>(null, false, validationError));

        try
        {
            var result = await _aggregatedApiService.GetAggregatedResultsAsync(request);
            return Ok(new ApiResponse<AggregatedResult>(result, "Aggregated data fetched successfully."));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to get aggregated results.");
            return StatusCode(500, new ApiResponse<AggregatedResult>(null, false, "Internal server error."));
        }
    }
    
    private static string? ValidateRequest(AggregatedRequest request)
    {
        if (request.Latitude is < -90 or > 90)
            return "Latitude must be between -90 and 90.";

        if (request.Longitude is < -180 or > 180)
            return "Longitude must be between -180 and 180.";

        if (request.Year < 1900 || request.Year > DateTime.UtcNow.Year)
            return "Year is out of valid range.";

        if (string.IsNullOrWhiteSpace(request.Query) || request.Query.Length < 3)
            return "Article query must be at least 3 characters long.";

        return null;
    }
}