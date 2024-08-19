namespace Ehrms.Shared;

public sealed class GenerateJwtRequest
{
    public string Username { get; set; } = string.Empty;
    public IEnumerable<string?> Roles { get; set; } = [];
}