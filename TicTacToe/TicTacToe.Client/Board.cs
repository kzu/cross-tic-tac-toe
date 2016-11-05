using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mqtt;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Library
{
    public enum Shape
    {
        Empty,
        Nought,
        Cross,
    }

    public class Board
    {
        static readonly string host = "192.168.0.200";
        static readonly int port = 55555;
        static readonly string topic = "tictactoe/game";
        static readonly Lazy<Board> instance;

        readonly IList<Tuple<int, int, Shape>> items;
        string clientId;
        IMqttClient client;

        static Board()
        {
            instance = new Lazy<Board>(() => new Board());
        }

        private Board()
        {
            items = new List<Tuple<int, int, Shape>>();

            InitializeAsync().Wait();
        }

        public event EventHandler Reloaded;

        public static Board Default => instance.Value;

        public async Task PlayAsync(int coordinateX, int coordinateY)
        {
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

        public Shape GetValue(int coordinateX, int coordinateY)
        {
            var item = items
                .FirstOrDefault(i => i.Item1 == coordinateX && i.Item2 == coordinateX);

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
                var shape = default(Shape);

                if (int.TryParse(values[0], out coordinateX) &&
                    int.TryParse(values[1], out coordinateY) &&
                    Enum.TryParse(values[2], out shape))
                {
                    items.Add(Tuple.Create(coordinateX, coordinateY, shape));
                }
            }

            Reloaded?.Invoke(this, EventArgs.Empty);
        }

        async Task InitializeAsync()
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

        string GetClientId() => Guid.NewGuid().ToString().Split(new string[] { "-" }, StringSplitOptions.None).First();

        Shape GetShape() => items.Count % 2 == 0 ? Shape.Nought : Shape.Cross;
    }
}
