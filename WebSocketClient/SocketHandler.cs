using System.Net.WebSockets;
using System.Text;

namespace WebSocketClient;

public static class SocketHandler
{
    public static void  AddSocketHandler(WebSocket webSocket)
    {
        try
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result;
            while (!webSocket.CloseStatus.HasValue)
            {
                result = webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).GetAwaiter().GetResult();
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Received: {message}");
                    var responseBytes = Encoding.UTF8.GetBytes($"Server Echo: {message}");
                    webSocket.SendAsync(new ArraySegment<byte>(responseBytes, 0, responseBytes.Length),
                        WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {
                    webSocket.CloseAsync(webSocket.CloseStatus!.Value, webSocket.CloseStatusDescription,
                        CancellationToken.None);
                    Console.WriteLine($"Connection closed: {webSocket.CloseStatus} - {webSocket.CloseStatusDescription}");
                }
            }
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }
}


