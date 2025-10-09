using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;

const string host = "localhost";
const string queue = "laba3";
const int port = 5672;
const string userName = "guest";
const string password = "guest";

Console.WriteLine("Введите текст сообщения:");
var text = Console.ReadLine();

if (string.IsNullOrWhiteSpace(text))
{
    Console.Error.WriteLine("Ошибка: текст сообщения пустой.");
    return;
}

var factory = new ConnectionFactory
{
    HostName = host,
    Port = port,
    UserName = userName,
    Password = password,
    RequestedConnectionTimeout = TimeSpan.FromSeconds(3)
};

try
{
    await using var connection = await factory.CreateConnectionAsync();
    await using var channel = await connection.CreateChannelAsync();

    await channel.QueueDeclareAsync(
        queue: queue,
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null);

    var body = Encoding.UTF8.GetBytes(text);

    await channel.BasicPublishAsync(
        exchange: string.Empty,
        routingKey: queue,
        mandatory: true,
        basicProperties: new BasicProperties
        {
            Persistent = true
        },
        body: body);

    Console.WriteLine("Успешная отправка.");
}
catch (BrokerUnreachableException)
{
    Console.WriteLine("Предупреждение: брокер недоступен.");
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Ошибка при отправке: {ex.Message}");
}
