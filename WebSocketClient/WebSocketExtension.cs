namespace WebSocketClient;

public static class WebSocketExtension
{
   public static WebApplication AddWebSocket(this WebApplication app)
   {
      app.Map("/api/socket", async context =>
      {
         if(context.WebSockets.IsWebSocketRequest) 
         {
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
           SocketHandler.AddSocketHandler(webSocket);
         }
      });

      return app;
   } 
}
