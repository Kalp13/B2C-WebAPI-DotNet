using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.Extensions.Logging.ApplicationInsights;
using TaskWebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IAzureAdLookupService, AzureAdLookupService>();
builder.Services.AddTransient<IFederationResolver, FederationResolver>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.AddApplicationInsights(
        configureTelemetryConfiguration: (config) =>
            config.ConnectionString = builder.Configuration.GetValue<string>("ApplicationInsights:ConnectionString"),
            configureApplicationInsightsLoggerOptions: (options) => { });

builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("validationController", LogLevel.Trace);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
