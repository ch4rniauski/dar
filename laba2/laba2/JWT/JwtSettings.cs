namespace laba2.JWT;

public sealed class JwtSettings
{
    public string SymmetricKey { get; set; } = string.Empty;
    
    public int ExpiresInMins { get; set; }
}
