using ApiAggregation.Application.Interface;
using ApiAggregation.Model.Statistics;
using Microsoft.AspNetCore.Mvc;
using Model.ApiResponse;

namespace ApiAggregation.Controllers;

[ApiController]
[Route("api/statistics")]
public class StatisticsController : ControllerBase
{
    private readonly IRequestStatsStore _statsStore;

    public StatisticsController(IRequestStatsStore statsStore)
    {
        _statsStore = statsStore;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var stats = _statsStore.GetAllStats();
        return Ok(new ApiResponse<IEnumerable<ApiStatisticsDto>>(stats));
    }
}