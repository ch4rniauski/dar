using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace laba2.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class GiphyController : ControllerBase
{
    private readonly IHttpClientFactory _http;
    private const string _apiKey = "noeUtiAhDdE2D7o4wAnHN40Kubt4efeF";

    public GiphyController(IHttpClientFactory http)
    {
        _http = http;
    }

    [Authorize]
    [HttpGet("search/{q}/{limit:int?}")]
    public async Task<IActionResult> Search(string q, int limit = 1)
    {
        var url = $"https://api.giphy.com/v1/gifs/search?api_key={_apiKey}&q={Uri.EscapeDataString(q)}&limit={limit}";
        var client = _http.CreateClient();
        
        var resp = await client.GetFromJsonAsync<JsonElement>(url);
        
        var gifs = resp.GetProperty("data")
            .EnumerateArray()
            .Select(d => new
            {
                Url = d.GetProperty("images")
                    .GetProperty("original")
                    .GetProperty("url")
                    .GetString()
            });
        
        var html = gifs.Aggregate(
            "<html><body>",
            (current, gif) => current + $"<img src='{gif.Url}' style='max-width:200px; margin:5px;' />");

        html += "</body></html>";

        return Content(html, "text/html");
    }
}
