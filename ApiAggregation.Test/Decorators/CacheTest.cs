using ApiAggragation.Infrastructure.Decorator;
using ApiAggregation.Application.Interfaces;
using ApiAggregation.Model.Dto;
using ApiAggregation.Model.Request;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace ApiAggregation.Test;

public class CacheTest
{
    [Fact]
    public async Task FetchAsync_ReturnsCachedValue_OnSecondCall()
    {
        // Arrange
        var request = new WeatherApiRequest();
        var response = new WeatherDto();
        var mockService = new Mock<IExternalApiService<WeatherApiRequest, WeatherDto>>();
        mockService.Setup(s => s.FetchAsync(request)).ReturnsAsync(response);

        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var logger = new Mock<ILogger<ExternalApiServiceCachingDecorator<WeatherApiRequest, WeatherDto>>>();

        var decorator = new ExternalApiServiceCachingDecorator<WeatherApiRequest, WeatherDto>(
            mockService.Object, memoryCache, logger.Object);

        // Act
        var first = await decorator.FetchAsync(request);
        var second = await decorator.FetchAsync(request);

        // Assert
        Assert.Equal(first, second);
        mockService.Verify(s => s.FetchAsync(request), Times.Once); // Should only be called once
    }
}