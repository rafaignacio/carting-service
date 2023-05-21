using CartingService.API.BackgroundServices;
using CartingService.API.Configurations;
using CartingService.API.Core;
using CatalogService.API.Security;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAllEndpointDefinitions();

builder.Services.AddAuthSecurity(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cart v1", Version = "v1" });
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "Cart v2", Version = "v2" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });
});

builder.Services.Configure<QueueConfiguration>(
    builder.Configuration.GetSection(QueueConfiguration.Name));

builder.Services.AddHostedService<ItemChangedConsumer>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpointDefinitions();

app.UseSwagger();
app.UseSwaggerUI( c =>
{
    c.SwaggerEndpoint("./v1/swagger.json", "v1");
    c.SwaggerEndpoint("./v2/swagger.json", "v2");
});

app.Run();

public partial class Program { }