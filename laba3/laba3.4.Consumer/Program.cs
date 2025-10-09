using Confluent.Kafka;
using System.Text.Json;

const string topic = "vehicle-location";
const string bootstrapServers = "localhost:9092";
const string groupId = "simple-consumer";

var config = new ConsumerConfig
{
    BootstrapServers = bootstrapServers,
    GroupId = groupId,
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using var consumer = new ConsumerBuilder<string, string>(config).Build();

consumer.Subscribe(topic);

Console.WriteLine("Консьюмер запущен...");

while (true)
{
    var cr = consumer.Consume();
    var evt = JsonSerializer.Deserialize<LocationEvent>(cr.Message.Value);

    Console.WriteLine($"Получено: {evt!.VehicleId} {evt.Latitude:F4},{evt.Longitude:F4} {evt.SpeedKmh:F1} км/ч");
}
