using ApiAggregation.Application.Helper;
using ApiAggregation.Model.Dto;
using Xunit;
using Assert = Xunit.Assert;

namespace ApiAggregation.Test.Helpers;

public class AggregationHelperTest
{
    [Fact]
    public void ApplyFilteringAndSorting_FiltersCalendarByDate()
    {
        // Arrange
        var targetDate = new DateTime(2024, 12, 25);
        var result = new AggregatedResult
        {
            Calendar = new CalendarDto
            {
                Holidays = new List<HolidayData>
                {
                    new HolidayData { Date = new DateTime(2024, 12, 25) },
                    new HolidayData { Date = new DateTime(2024, 1, 1) }
                }
            }
        };

        var request = new AggregatedRequest
        {
            FilterDate = targetDate
        };

        // Act
        var filtered = AggregationHelper.ApplyFilteringAndSorting(result, request);

        // Assert
        Assert.Single(filtered.Calendar.Holidays);
        Assert.Equal(targetDate, filtered.Calendar.Holidays.ToList()[0].Date);
    }

    [Fact]
    public void ApplyFilteringAndSorting_FiltersNewsByCountry()
    {
        // Arrange
        var result = new AggregatedResult
        {
            News = new NewsArticleDto
            {
                Articles = new List<Article>
                {
                    new Article { Sourcecountry = "US" },
                    new Article { Sourcecountry = "UK" }
                }
            }
        };

        var request = new AggregatedRequest
        {
            CountryArticle = "UK"
        };

        // Act
        var filtered = AggregationHelper.ApplyFilteringAndSorting(result, request);

        // Assert
        Assert.Single(filtered.News.Articles);
        Assert.Equal("UK", filtered.News.Articles.ToList()[0].Sourcecountry);
    }

    [Fact]
    public void ApplyFilteringAndSorting_SortsNewsByTitle()
    {
        // Arrange
        var result = new AggregatedResult
        {
            News = new NewsArticleDto
            {
                Articles = new List<Article>
                {
                    new Article { Title = "Zebra" },
                    new Article { Title = "Apple" },
                }
            }
        };

        var request = new AggregatedRequest
        {
            Sort = true
        };

        // Act
        var sorted = AggregationHelper.ApplyFilteringAndSorting(result, request);

        // Assert
        Assert.Equal("Apple", sorted.News.Articles.ToList()[0].Title);
        Assert.Equal("Zebra", sorted.News.Articles.ToList()[1].Title);
    }

    [Fact]
    public void ApplyFilteringAndSorting_DoesNothingWhenNoFilters()
    {
        // Arrange
        var holidays = new List<HolidayData> { new HolidayData { Date = DateTime.Now } };
        var articles = new List<Article> { new Article { Sourcecountry = "US", Title = "B" } };

        var result = new AggregatedResult
        {
            Calendar = new CalendarDto { Holidays = holidays },
            News = new NewsArticleDto { Articles = articles }
        };

        var request = new AggregatedRequest(); // no filters

        // Act
        var unchanged = AggregationHelper.ApplyFilteringAndSorting(result, request);

        // Assert
        Assert.Equal(holidays, unchanged.Calendar.Holidays);
        Assert.Equal(articles, unchanged.News.Articles);
    }
}