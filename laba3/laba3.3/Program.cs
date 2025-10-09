using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

const string host = "localhost";
const string queue = "laba3";
const int port = 5672;
const string userName = "guest";
const string password = "guest";

var factory = new ConnectionFactory
{
    HostName = host,
    Port = port,
    UserName = userName,
    Password = password,
    RequestedConnectionTimeout = TimeSpan.FromSeconds(3)
};

await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(
    queue: queue,
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

var consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync += async (sender, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    
    var obj = JsonSerializer.Deserialize<WeatherResponse>(message);

    Console.WriteLine($"Температура: {obj?.Current.Temperature}");
    
    await ((AsyncEventingBasicConsumer)sender).Channel.BasicAckAsync(
        deliveryTag: eventArgs.DeliveryTag,
        multiple: false);
};

await channel.BasicConsumeAsync(
    queue: queue,
    consumer: consumer,
    autoAck: false);

Console.WriteLine("Consumer запущен");
Console.ReadLine();
