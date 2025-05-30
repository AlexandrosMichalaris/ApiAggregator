using ApiAggragation.Infrastructure.Configuration;
using ApiAggregation.Configuration.DI;
using ApiAggregation.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<OpenWeatherSettings>(builder.Configuration.GetSection("ExternalApis:OpenWeather"));
builder.Services.Configure<NewsSettings>(builder.Configuration.GetSection("ExternalApis:OpenWeather"));
builder.Services.Configure<CalendarSettings>(builder.Configuration.GetSection("ExternalApis:OpenWeather"));

// Configure services using the static class method
builder.Services.ConfigureServices();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddHttpClient();



// Replace default logging with Serilog and Read Serilog config from appsettings.json
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>(); // Register the exception middleware

app.Run();