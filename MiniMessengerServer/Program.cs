using Microsoft.Extensions.Logging;
using System;

namespace MiniMessengerServer
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleLogging.Log("Server start");
            var messenger = new MessengerManager();
            var ws = new Webserver("http://localhost:5000/",
                request =>
                {
                    Console.WriteLine("Received request:");
                    Console.WriteLine($"RawUrl     => {request.RawUrl}");
                    Console.WriteLine($"HttpMethod => {request.HttpMethod}");

                    return messenger.GetData(request.Url.AbsolutePath, request.QueryString);
                });

            ws.Run();
            Console.WriteLine("Press a key to quit.");
            Console.ReadKey();
            ws.Stop();

            SimpleLogging.Log("Server stopped");
        }
    }
}
