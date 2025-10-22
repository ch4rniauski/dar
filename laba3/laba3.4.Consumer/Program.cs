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
    var consumeResult = consumer.Consume();
    var eventArgs = JsonSerializer.Deserialize<LocationEvent>(consumeResult.Message.Value);

    Console.WriteLine($"Получено: {eventArgs!.VehicleId} {eventArgs.Latitude:F4},{eventArgs.Longitude:F4} {eventArgs.SpeedKmh:F1} км/ч");
}
