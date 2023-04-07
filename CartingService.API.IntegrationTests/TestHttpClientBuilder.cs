using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace CartingService.API.IntegrationTests;

public static class TestHttpClientBuilder
{
    public static HttpClient CreateClient()
    {
        var web = new WebApplicationFactory<Program>();

        return web.WithWebHostBuilder(builder =>
        {
            builder.UseTestServer();
        }).CreateClient();
    }
}
