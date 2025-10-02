namespace JsonPlaceholder.Models;

internal sealed class User
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Website { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string Username { get; set; } = string.Empty;
    
    public Address? Address { get; set; }
    
    public Company? Company { get; set; }
}
