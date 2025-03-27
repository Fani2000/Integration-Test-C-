using Microsoft.AspNetCore.Mvc.Testing;

namespace Tests;

public class BaseWebApplicationSetup<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Pass all the setup logic
    }
}