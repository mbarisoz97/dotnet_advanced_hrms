﻿namespace Ehrms.Shared;

public sealed class GenerateTokenResponse
{
	public string Username { get; set; } = string.Empty;
	public string AccessToken { get; set; } = string.Empty;
	public string RefreshToken { get; set; } = string.Empty;
	public DateTime ExpiresIn { get; set; }
}