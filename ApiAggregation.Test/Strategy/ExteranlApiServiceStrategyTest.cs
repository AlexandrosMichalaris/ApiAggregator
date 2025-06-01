using ApiAggregation.Application.Interfaces;
using ApiAggregation.Application.Strategy;
using ApiAggregation.Model.Dto;
using ApiAggregation.Model.Request;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace ApiAggregation.Test.Strategy;

public class ExteranlApiServiceStrategyTest
{
    [Fact]
    public async Task GetExternalApiService_ReturnsCorrectService()
    {
        // Arrange
        var mockWeatherService = new Mock<IExternalApiService<WeatherApiRequest, WeatherDto>>();
        var serviceProviderMock = new Mock<IServiceProvider>();

        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IExternalApiService<WeatherApiRequest, WeatherDto>)))
            .Returns(mockWeatherService.Object);

        var strategy = new ExternalApiServiceStrategy(serviceProviderMock.Object);

        // Act
        var result = await strategy.GetExternalApiService<WeatherApiRequest, WeatherDto>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(mockWeatherService.Object, result);
    }

    [Fact]
    public async Task GetExternalApiService_ThrowsIfServiceNotFound()
    {
        // Arrange
        var serviceProviderMock = new Mock<IServiceProvider>();

        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IExternalApiService<WeatherApiRequest, WeatherDto>)))
            .Returns(null);

        var strategy = new ExternalApiServiceStrategy(serviceProviderMock.Object);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await strategy.GetExternalApiService<WeatherApiRequest, WeatherDto>());

        Assert.Contains("No external API service found", ex.Message);
    }
}