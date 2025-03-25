using System.Net.WebSockets;
using System.Text;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Tests;

public class WebSocketIntegrationTests(WebApplicationFactory<Program>? factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task WebSocket_Echo_Test()
    {
        // ARRANGE
        // Create test HTTP client attached to the in-memory test server
        var client = factory?.Server.CreateWebSocketClient();

        // Construct WebSocket URI based on the in-memory test environment
        var serverUri = new Uri("ws://localhost/ws");

        // Establish WebSocket connection via the server-side context
        using var webSocket = await client?.ConnectAsync(serverUri, CancellationToken.None)!;

        // Send a message to the WebSocket server
        var message = "Hello, WebSocket!";
        var messageBytes = Encoding.UTF8.GetBytes(message);
        await webSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);

        // ACT
        var buffer = new byte[1024 * 4];
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        var response = Encoding.UTF8.GetString(buffer, 0, result.Count);

        // Close the WebSocket connection
        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Test Completed", CancellationToken.None);

        // ASSERT
        Assert.Equal(WebSocketState.Closed, webSocket.State); // Ensure WebSocket is closed properly
        Assert.Equal($"Server Echo: {message}", response); // Ensure response matches the server's echo behavior
    }
}