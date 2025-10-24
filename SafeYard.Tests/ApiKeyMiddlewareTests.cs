using Xunit;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SafeYard;
using System.Collections.Generic;

public class ApiKeyMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ApiKeyInvalida_Retorna401()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string> { { "SafeYardApiKey", "123" } })
            .Build();

        var context = new DefaultHttpContext();
        context.Request.Headers["X-API-KEY"] = "errada";
        var middleware = new ApiKeyMiddleware((innerHttpContext) => Task.CompletedTask, config);

        await middleware.InvokeAsync(context);

        Assert.Equal(401, context.Response.StatusCode);
    }
}