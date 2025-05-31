using ApiAggregation.Model.Dto;

namespace ApiAggregation.Application.Helper;

public static class AggregationHelper
{
    public static AggregatedResult ApplyFilteringAndSorting(AggregatedResult result, AggregatedRequest request)
    {
        // if (result == null) throw new ArgumentNullException(nameof(result));
        // if (request == null) throw new ArgumentNullException(nameof(request));

        FilterCalendarData(result, request.FilterDate);
        FilterNewsArticlesByCountry(result, request.CountryArticle);
        SortNewsArticlesByTitle(result, request.Sort);

        return result;
    }

    private static void FilterCalendarData(AggregatedResult result, DateTime? filterDate)
    {
        if (filterDate.HasValue && result.Calendar?.Holidays != null)
        {
            result.Calendar.Holidays = result.Calendar.Holidays
                .Where(d => d.Date == filterDate.Value)
                .ToList();
        }
    }

    private static void FilterNewsArticlesByCountry(AggregatedResult result, string? country)
    {
        if (!string.IsNullOrWhiteSpace(country) && result.News?.Articles != null)
        {
            result.News.Articles = result.News.Articles
                .Where(a => a.Sourcecountry.Contains(country))
                .ToList();
        }
    }

    private static void SortNewsArticlesByTitle(AggregatedResult result, bool? sort)
    {
        if (sort == true && result.News?.Articles != null)
        {
            result.News.Articles = result.News.Articles
                .OrderBy(a => a.Title)
                .ToList();
        }
    }
}