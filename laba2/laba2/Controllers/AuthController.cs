using laba2.Contracts.JWT;
using laba2.DTO;
using laba2.Models;
using Microsoft.AspNetCore.Mvc;

namespace laba2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenProvider _tokenProvider;
    private static readonly List<User> _users = [];

    public AuthController(ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] UserToCreateDto request)
    {
        if (_users.Any(u => u.Username == request.Username))
        {
            return BadRequest("User already exists");
        }

        var user = new User
        {
            Username = request.Username,
            Password = request.Password,
        };

        _users.Add(user);
        
        return Ok("Registered successfully");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserToLoginDto request)
    {
        var user = _users.FirstOrDefault(u => u.Username == request.Username
                                              && u.Password == request.Password);

        if (user == null)
        {
            return Unauthorized();
        }

        var accessToken = _tokenProvider.GenerateAccessToken();

        return Ok(new
        {
            access_token = accessToken
        });
    }
}
