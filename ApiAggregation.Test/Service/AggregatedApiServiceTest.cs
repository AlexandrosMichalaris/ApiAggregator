using ApiAggregation.Application.Interfaces;
using ApiAggregation.Application.Service;
using ApiAggregation.Application.Strategy;
using ApiAggregation.Model.Dto;
using ApiAggregation.Model.Request;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace ApiAggregation.Test.Service;

public class AggregatedApiServiceTest
{
    [Fact]
    public async Task GetAggregatedResultsAsync_CallsAllServicesAndReturnsResult()
    {
        // Arrange
        var newsApiMock = new Mock<IExternalApiService<NewsApiRequest, NewsArticleDto>>();
        newsApiMock
            .Setup(s => s.FetchAsync(It.IsAny<NewsApiRequest>()))
            .ReturnsAsync(new NewsArticleDto { Articles = new List<Article>() });

        var weatherApiMock = new Mock<IExternalApiService<WeatherApiRequest, WeatherDto>>();
        weatherApiMock
            .Setup(s => s.FetchAsync(It.IsAny<WeatherApiRequest>()))
            .ReturnsAsync(new WeatherDto());

        var calendarApiMock = new Mock<IExternalApiService<CalendarApiRequest, CalendarDto>>();
        calendarApiMock
            .Setup(s => s.FetchAsync(It.IsAny<CalendarApiRequest>()))
            .ReturnsAsync(new CalendarDto());

        // Mock IServiceProvider
        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IExternalApiService<NewsApiRequest, NewsArticleDto>)))
            .Returns(newsApiMock.Object);
        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IExternalApiService<WeatherApiRequest, WeatherDto>)))
            .Returns(weatherApiMock.Object);
        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IExternalApiService<CalendarApiRequest, CalendarDto>)))
            .Returns(calendarApiMock.Object);

        var strategy = new ExternalApiServiceStrategy(serviceProviderMock.Object);

        var loggerMock = new Mock<ILogger<AggregatedApiService>>();
        var cacheMock = new Mock<IMemoryCache>();

        var service = new AggregatedApiService(strategy, loggerMock.Object, cacheMock.Object);

        var request = new AggregatedRequest
        {
            CountryCode = "US",
            Year = 2024,
            Latitude = 40.7128,
            Longitude = -74.0060
        };

        // Act
        var result = await service.GetAggregatedResultsAsync(request);

        // Assert
        Assert.NotNull(result);
    }
}