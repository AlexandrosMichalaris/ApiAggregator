using ApiAggragation.Infrastructure.Decorator;
using ApiAggregation.Application.Interfaces;
using ApiAggregation.Application.Service;
using ApiAggregation.Model.Constants;
using ApiAggregation.Model.Dto;
using ApiAggregation.Model.Request;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace ApiAggregation.Test;

public class StatsTest
{
    [Fact]
    public async Task FetchAsync_StoresRequestStatistics()
    {
        // Arrange
        var request = new WeatherApiRequest();
        var response = new WeatherDto();

        var mockInner = new Mock<IExternalApiService<WeatherApiRequest, WeatherDto>>();
        mockInner.Setup(x => x.FetchAsync(request)).ReturnsAsync(response);
        mockInner.Setup(x => x.Name).Returns(Constants.WeatherName);

        var statsStore = new RequestStatsStore();
        var logger = new Mock<ILogger<ExternalApiServiceStatsDecorator<WeatherApiRequest, WeatherDto>>>();

        var decorator = new ExternalApiServiceStatsDecorator<WeatherApiRequest, WeatherDto>(
            mockInner.Object, statsStore, logger.Object);

        // Act
        await decorator.FetchAsync(request);

        // Assert
        var allStats = statsStore.GetAllStats();
        Assert.Single(allStats); // One entry in the dictionary
        Assert.True(allStats.ToList()[0].ApiName == Constants.WeatherName);
    }
}