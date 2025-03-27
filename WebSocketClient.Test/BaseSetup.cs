using Microsoft.AspNetCore.Mvc.Testing;

namespace Tests;

public class BaseWebApplicationSetup<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // TODO: Add other services you want to import
        builder.UseEnvironment("Testing");
    }
}