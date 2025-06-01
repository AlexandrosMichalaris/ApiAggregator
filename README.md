# API Aggregation Service

An extensible ASP.NET Core service that aggregates data from multiple external APIs (Weather, News, Calendar), applies filtering/sorting, and returns a unified response.

## 🔍 Interactive API Docs (Swagger)

You can view and test all endpoints using the Swagger UI:

**URL:** [/swagger/index.html](http://localhost:5078/swagger/index.html)

Swagger provides a web-based interface to explore the API, try out requests, and see response examples.

---
## 🔗 API Endpoints

### `GET /api/aggregation/get`

Fetches aggregated results from external services based on location, year, country and Search Query. Optional filters can be applied for calendar date and news article attributes.

#### Query Parameters

| Name             | Type   | Required | Description                                                                               |
| ---------------- | ------ | -------- | ----------------------------------------------------------------------------------------- |
| `latitude`       | float  | Yes      | Latitude for weather data                                                                 |
| `longitude`      | float  | Yes      | Longitude for weather data                                                                |
| `year`           | int    | Yes      | Year used to fetch holidays from calendar API                                             |
| `countryCode`    | string | Yes      | Country code for calendar API (e.g., `"GR"`)                                              |
| `filterDate`     | string | No       | Filter calendar holidays by date (e.g., `"2025-03-03"`)                                   |
| `sort`           | bool   | No       | Sort news articles alphabetically by title                                                |
| `countryarticle` | string | No       | Filter news articles by source country name                                               |
| `query`          | string | Yes      | Search keyword for news articles (e.g., `"camel"`). It needs to be more than 3 characters |

####  Sample Request

```http
GET http://localhost:5078/api/aggregation/get?latitude=42&longitude=52&year=20240&countryCode=GR&filterDate=2025-03-03&sort=true&countryarticle=Arab&query=camel
```

#### Sample Response
```json
{
  "data": {
    "weather": {
      "temperature": 19.7,
      "timezone": "GMT",
      "time": "5/31/2025 9:15:00 PM",
      "weatherCode": 0,
      "success": false
    },
    "calendar": {
      "holidays": "holidays": [
		{
			"fixed": false,
			"localName": "Πρωτοχρονιά",
			"name": "New Year's Day",
			"global": "True",
			"date": "2024-01-01T00:00:00"
		},
		{
			"fixed": false,
			"localName": "Θεοφάνεια",
			"name": "Epiphany",
			"global": "True",
			"date": "2024-01-06T00:00:00"
		}
    },
    "news": {
      "articles": [
        {
          "title": "Naik Unta di Tepi Pantai Kelan Bali Jadi Buruan Wisatawan Saat Liburan",
          "url": "https://www.beritasatu.com/...",
          "language": "Indonesian",
          "sourcecountry": "Indonesia"
        },
        {
          "title": "الهجن يختتم دورة التسجيل | صحيفة الرياضية",
          "url": "https://arriyadiyah.com/...",
          "language": "Arabic",
          "sourcecountry": "Saudi Arabia"
        }
      ]
    },
    "timestamp": "2025-06-01T00:15:33.750928+03:00"
  },
  "success": true,
  "message": "Aggregated data fetched successfully.",
  "eventId": null
}
```

### `GET /api/statistics`

Returns runtime performance and usage statistics for each external API used by the aggregation service. This endpoint helps monitor latency and track API health.

####  Sample Request

```http
GET http://localhost:5078/api/statistics
```

#### Sample Response
```json
{
  "data": [
    {
      "apiName": "Calendar",
      "totalRequests": 1,
      "averageResponseTime": 446,
      "performanceBucket": "Average"
    },
    {
      "apiName": "Weather",
      "totalRequests": 1,
      "averageResponseTime": 625,
      "performanceBucket": "Slow"
    },
    {
      "apiName": "News",
      "totalRequests": 1,
      "averageResponseTime": 1441,
      "performanceBucket": "Slow"
    }
  ],
  "success": true,
  "message": "Statistics data fetched successfully.",
  "eventId": null
}
```

#### Performance Buckets

| Bucket  | Threshold (ms) |
|---------|----------------|
| Fast    | < 300          |
| Average | 300–700        |
| Slow    | > 700          |

---
## Fallback Mechanism

This project includes a **fallback mechanism** to improve fault tolerance and ensure graceful degradation when external APIs fail or become temporarily unavailable.

- When a request fails (e.g., network error, timeout, or 5xx response), the system attempts to:
    1. **Return cached data** (if caching is enabled).
    
    2. Return an empty object (So it will not interfere with the data of the other endpoints)
    
    3. **Log the failure** using Serilog.

---
## Architecture Overview

This project follows **Clean Architecture** principles, emphasizing separation of concerns, testability, and modularity. It leverages several design patterns and a modular service structure to support flexible, maintainable API integrations.

### Design Patterns Used

- **Strategy Pattern**  
    Dynamically resolves the appropriate API implementation at runtime based on the request context.

- **Decorator Pattern**  
    Enables optional behaviors like **caching** and **statistics tracking** to be layered around core services.

- **Dependency Injection**  
    Promotes loose coupling by making components interfaces injectable in the constructor of each dependency.


|Component|Role|
|---|---|
|`IExternalApiService<TRequest, TResponse>`|Interface implemented by each API client (Weather, News, Calendar).|
|`ExternalApiServiceCachingDecorator`|Adds caching on top of API clients.|
|`ExternalApiServiceStatsDecorator`|Tracks response time and request count.|
|`ExternalApiServiceStrategy`|Resolves the correct service instance based on generic type.|
|`AggregatedApiService`|Orchestrates fetching from multiple services and aggregates results.|
|`AggregationHelper`|Filters and sorts results based on request.|
|`RequestStatsStore`|In-memory storage for performance data.|
|`AggregationController`|Exposes REST endpoints.|

#### Decorator Chain

Each service is wrapped with decorators like so:

[StatsDecorator] -> [CachingDecorator] -> [ApiService]

Decorators are registered using `services.Decorate<>()` to compose behavior dynamically while keeping concerns separate.

---
## Configuration (`appsettings.json`)

This project uses an `appsettings.json` file to manage logging, external API URLs, and other environment-specific settings. This ensures the application is **easy to configure, extend, and deploy across environments**.

#### Serilog Logging Configuration

```json
"Serilog": {
  "Using": [ "Serilog.Sinks.File" ],
  "MinimumLevel": "Information",
  "WriteTo": [
    {
      "Name": "File",
      "Args": {
        "path": ".../logs/log-.txt",
        "rollingInterval": "Day",
        "retainedFileCountLimit": 7
      }
    }
  ]
}
```

- Uses **Serilog** for structured and file-based logging.
    
- Logs are written to a daily-rolling file with a 7-day retention policy.
    
- Useful for diagnostics, debugging, and tracing production issues.
    
- Customizable to use other sinks like Console, Seq, or Elasticsearch.

### External API Settings

```json
"ApiSettings": {
  "OpenWeather": {
    "BaseUrl": "https://api.open-meteo.com/v1/forecast"
  },
  "NewsArticles": {
    "BaseUrl": "https://api.gdeltproject.org/api/v2/doc/doc"
  },
  "Calendar": {
    "BaseUrl": "https://date.nager.at/api/v3/PublicHolidays"
  }
}
```

- Stores **base URLs** for external API integrations.

- These settings are injected via `IOptions<>` to avoid hardcoding and support environment-based overrides.

- Keeps all endpoint configurations centralized and maintainable.
