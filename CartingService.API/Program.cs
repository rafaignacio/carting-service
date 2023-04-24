using CartingService.API.BackgroundServices;
using CartingService.API.Configurations;
using CartingService.API.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAllEndpointDefinitions();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Cart v1", Version = "v1" });
    c.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Cart v2", Version = "v2" });
});

builder.Services.Configure<QueueConfiguration>(
    builder.Configuration.GetSection(QueueConfiguration.Name));

builder.Services.AddHostedService<ItemChangedConsumer>();

var app = builder.Build();

app.UseEndpointDefinitions();

app.UseSwagger();
app.UseSwaggerUI( c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
});

app.Run();

public partial class Program { }