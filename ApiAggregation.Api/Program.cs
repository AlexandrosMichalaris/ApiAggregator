using ApiAggregation.Configuration.DI;
using ApiAggregation.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

// Replace default logging with Serilog and Read Serilog config from appsettings.json
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseRouting();
app.MapControllers();

// Configure services using the static class method
builder.Services.ConfigureServices();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>(); // Register the exception middleware

app.Run();