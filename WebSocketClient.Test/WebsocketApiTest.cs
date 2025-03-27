using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Tests;

public class WebSocketIntegrationTests(BaseWebApplicationSetup<Program> factory) : IClassFixture<BaseWebApplicationSetup<Program>>
{
    private readonly BaseWebApplicationSetup<Program> factory = factory;

    [Fact]
    public async Task WebSocket_Echo_Test()
    {
        // ARRANGE
        // Construct WebSocket URI based on the in-memory test environment
        Microsoft.AspNetCore.TestHost.WebSocketClient client = factory.Server.CreateWebSocketClient();
        
        var serverUri = new Uri("ws://localhost/api/socket");

        // Establish WebSocket connection via the server-side context
        using var webSocket = await client.ConnectAsync(serverUri, CancellationToken.None)!;

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