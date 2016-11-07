using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mqtt;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Client
{
    public struct Shape
    {
        public static readonly string Empty = "";
        public static readonly string Nought = "O";
        public static readonly string Cross = "X";
    }

    public class Board
    {
        static readonly string host = "192.168.1.120";
        static readonly int port = 55555;
        static readonly string topic = "tictactoe/game";
        static readonly Lazy<Board> instance;

        readonly IList<Tuple<int, int, string>> items;
        string clientId;
        IMqttClient client;

        static Board()
        {
            instance = new Lazy<Board>(() => new Board());
        }

        private Board()
        {
            items = new List<Tuple<int, int, string>>();

            InitializeAsync().Wait();
        }

        public event EventHandler Reloaded;

        public event EventHandler GameCompleted;

        public static Board Default => instance.Value;

        public async Task PlayAsync(int coordinateX, int coordinateY)
        {
            if (client == null || !client.IsConnected)
            {
                return;
            }

            if (items.Any(i => i.Item1 == coordinateX && i.Item2 == coordinateY && i.Item3 != Shape.Empty))
            {
                return;
            }

            var shape = GetShape();
            var item = Tuple.Create(coordinateX, coordinateY, shape);

            items.Add(item);

            var payload = Encoding.UTF8.GetBytes(ToString());
            var message = new MqttApplicationMessage(topic, payload);

            await client
                .PublishAsync(message, MqttQualityOfService.AtLeastOnce)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public string GetValue(int coordinateX, int coordinateY)
        {
            var item = items
                .FirstOrDefault(i => i.Item1 == coordinateX && i.Item2 == coordinateY);

            return item == null ? Shape.Empty : item.Item3;
        }

        public override string ToString()
        {
            var serializedBoard = new StringBuilder();
            
            foreach(var item in items)
            {
                var value = $"{item.Item1}:{item.Item2}:{item.Item3}";

                serializedBoard.AppendLine(value);
            }

            return serializedBoard.ToString();
        }

        void Reload(string serializedBoard)
        {
            if (string.IsNullOrWhiteSpace(serializedBoard)) return;

            var lines = serializedBoard.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            if (!lines.Any()) return;

            items.Clear();

            foreach (var line in lines)
            {
                var values = line.Split(new string[] { ":" }, StringSplitOptions.None);
                var coordinateX = default(int);
                var coordinateY = default(int);

                if (int.TryParse(values[0], out coordinateX) &&
                    int.TryParse(values[1], out coordinateY))
                {
                    var shape = values[2];

                    items.Add(Tuple.Create(coordinateX, coordinateY, shape ?? string.Empty));
                }
            }

            if (items.Count == 9)
            {
                GameCompleted?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Reloaded?.Invoke(this, EventArgs.Empty);
            }
        }

        async Task InitializeAsync()
        {
            try
            {
                var config = new MqttConfiguration
                {
                    Port = port,
                    MaximumQualityOfService = MqttQualityOfService.AtLeastOnce,
                    AllowWildcardsInTopicFilters = true,
                    WaitTimeoutSecs = 10,
                    KeepAliveSecs = 15
                };

                clientId = GetClientId();
                client = await MqttClient
                    .CreateAsync(host, config)
                    .ConfigureAwait(continueOnCapturedContext: false);

                await client
                    .ConnectAsync(new MqttClientCredentials(clientId))
                    .ConfigureAwait(continueOnCapturedContext: false);

                client.MessageStream.Subscribe(message =>
                {
                    var serializedBoard = Encoding.UTF8.GetString(message.Payload);

                    Reload(serializedBoard);
                });

                await client
                    .SubscribeAsync(topic, MqttQualityOfService.AtLeastOnce)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                //TODO: Handle exception
            }
        }

        string GetClientId() => Guid.NewGuid().ToString().Split(new string[] { "-" }, StringSplitOptions.None).First();

        string GetShape() => items.Count % 2 == 0 ? Shape.Nought : Shape.Cross;
    }
}
