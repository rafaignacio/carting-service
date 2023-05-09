using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace CatalogService.API.Security;

public static class AuthExtensions
{
    public static IServiceCollection AddAuthSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication( options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer( options =>
        {
            var rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(
                source: Convert.FromBase64String(configuration["JwtConfig:Key"]!),
                bytesRead: out int _);

            options.Authority = configuration["JwtConfig:Authority"]!;
            options.RequireHttpsMetadata = false;

            options.TokenValidationParameters = new()
            {
                ValidAudience = configuration["JwtConfig:Audience"]!,
                ValidIssuer = configuration["JwtConfig:Issuer"]!,
                IssuerSigningKey = new RsaSecurityKey(rsa),

                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
            };
        });

        services.AddAuthorization( cfg =>
        {
            cfg.AddPolicy("default", policy => policy.RequireRole("manager", "buyer"));
        });

        return services;
    }
}
