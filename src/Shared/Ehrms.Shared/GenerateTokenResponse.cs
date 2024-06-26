namespace Ehrms.Shared;

public sealed class GenerateTokenResponse
{
    public string Username { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresIn { get; set; }
}