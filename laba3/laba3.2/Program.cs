using System.Text.Json;
using System.Text.Json.Serialization;
using RabbitMQ.Client;

const string host = "localhost";
const string queue = "laba3";
const int port = 5672;
const string userName = "guest";
const string password = "guest";

const string query = "Polatsk";
const string apiKey = "d8e29a8e25f51ef204b6a289c2eeeb3c";

const string apiUrl = $"https://api.weatherstack.com/current?access_key={apiKey}&query={query}";

using var http = new HttpClient();
using var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

using var resp = await http.SendAsync(request);

if (!resp.IsSuccessStatusCode)
{
    Console.Error.WriteLine($"Ошибка запроса погоды: {resp.StatusCode}");
    
    return;
}

var json = await resp.Content.ReadAsStringAsync();

var weather = JsonSerializer.Deserialize<WeatherResponse>(json);

if (weather is null)
{
    Console.Error.WriteLine("Ошибка: данные о погоде невалидны или пусты.");
    
    return;
}

Console.WriteLine($"Температура: {weather.Current.Temperature}");

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

var body = JsonSerializer.SerializeToUtf8Bytes(weather);

await channel.BasicPublishAsync(
    exchange: string.Empty,
    routingKey: queue,
    mandatory: true,
    basicProperties: new BasicProperties
    {
        Persistent = true
    },
    body: body);

Console.WriteLine("Погода отправлена.");

public sealed class WeatherResponse
{
    [JsonPropertyName("current")]
    public CurrentWeather Current { get; set; } = null!;
}

public sealed class CurrentWeather
{
    [JsonPropertyName("temperature")]
    public int Temperature { get; set; }
}
