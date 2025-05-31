namespace ApiAggregation.Model.Constants;

public static class Constants
{
    public const string WeatherName = "Weather";
    
    public const string CalendarName = "Calendar";

    public const string NewsName = "News";

    public const string LatitudeErrorMessage = "Latitude must be between -90 and 90.";
    
    public const string LongitudeErrorMessage = "Longitude must be between -180 and 180.";
    
    public const string YearErrorMessage = "Year is out of valid range.";

    public const string QueryErrorMessage = "Article query must be at least 3 characters long.";
}