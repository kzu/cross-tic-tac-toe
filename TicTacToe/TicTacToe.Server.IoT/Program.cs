using System.Diagnostics;
using System.Net.Mqtt;
using System.Threading;

namespace TicTacToe.Server.IoT
{
    class Program
    {
        static readonly int port = 55555;

        static void Main(string[] args)
        {
            Debug.WriteLine("Configuring Server...");

            var stopSignal = new ManualResetEventSlim();
            var config = new MqttConfiguration
            {
                Port = port,
                MaximumQualityOfService = MqttQualityOfService.AtLeastOnce,
                AllowWildcardsInTopicFilters = true,
                WaitTimeoutSecs = 10,
                KeepAliveSecs = 15
            };
            var server = MqttServer.Create(config);

            Debug.WriteLine("Starting Server...");

            server.Stopped += (sender, e) =>
            {
                Debug.WriteLine("Server stopped. Finishing app...");
                stopSignal.Set();
            };
            server.ClientConnected += (sender, e) => Debug.WriteLine($"New player connected: {e}");
            server.ClientDisconnected += (sender, e) => Debug.WriteLine($"Player disconnected: {e}");

            server.Start();

            Debug.WriteLine("Server started...");
            Debug.WriteLine("Listening for new players...");

            stopSignal.Wait();
        }
    }
}
