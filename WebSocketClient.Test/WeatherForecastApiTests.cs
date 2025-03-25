using System.Net;
using System.Text.Json;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Tests;

public class WeatherForecastApiTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task WeatherForecast_Endpoint_Returns_SuccessResponse_With_ValidData()
    {
        // ARRANGE
        var client = factory.CreateClient();

        // ACT
        var response = await client.GetAsync("/WeatherForecast");

        // ASSERT
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Ensure status code is 200 OK
        var responseContent = await response.Content.ReadAsStringAsync();

        // Deserialize JSON
        var weatherForecasts = JsonSerializer.Deserialize<WeatherForecast[]>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(weatherForecasts); // Ensure we got a valid response body
        Assert.NotEmpty(weatherForecasts); // Ensure there is at least one forecast in the response

        // Validate the contents of the WeatherForecast object
        foreach (var forecast in weatherForecasts)
        {
            Assert.True(forecast.TemperatureC != default); // Ensure temperature is present
            Assert.False(string.IsNullOrEmpty(forecast.Summary)); // Ensure summary is present
        }
    }

    // Define the WeatherForecast model to deserialize test response
    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
    }
}