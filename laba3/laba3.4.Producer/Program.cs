using Confluent.Kafka;
using System.Text.Json;

const string topic = "vehicle-location";
var config = new ProducerConfig
{
    BootstrapServers = "localhost:9092"
};

using var producer = new ProducerBuilder<string, string>(config).Build();

var rand = new Random();
var vehicles = new[]
{
    "Автомобиль №1",
    "Автомобиль №2",
    "Автомобиль №3"
};

for (var i = 0; i < 10; i++)
{
    var evt = new LocationEvent
    {
        VehicleId = vehicles[rand.Next(vehicles.Length)],
        Latitude = 55.48 + rand.NextDouble() * 0.01,
        Longitude = 28.80 + rand.NextDouble() * 0.01,
        SpeedKmh = 40 + rand.NextDouble() * 20,
        Timestamp = DateTimeOffset.UtcNow
    };

    var json = JsonSerializer.Serialize(evt);

    await producer.ProduceAsync(topic, new Message<string, string>
    {
        Key = evt.VehicleId,
        Value = json
    });

    Console.WriteLine($"Отправлено: {evt.VehicleId} {evt.Latitude:F4},{evt.Longitude:F4} {evt.SpeedKmh:F1} км/ч");
    
    await Task.Delay(3000);
}

public sealed class LocationEvent
{
    public string VehicleId { get; init; } = string.Empty;
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public double SpeedKmh { get; init; }
    public DateTimeOffset Timestamp { get; init; }
}
