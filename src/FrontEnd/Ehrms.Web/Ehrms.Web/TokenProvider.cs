using Ehrms.Shared;
using Microsoft.IdentityModel.Tokens;

namespace Ehrms.Web;

internal class TokenProvider : ITokenProvider
{
	private readonly ILogger<TokenProvider> _logger;
	private readonly IHttpClientFactory _factory;
	private readonly IEndpointProvider _endpointProvider;

	private string _currentAccessToken = "";

	public TokenProvider(ILogger<TokenProvider> logger, IHttpClientFactory factory, IEndpointProvider endpointProvider)
	{
		_logger = logger;
		_factory = factory;
		_endpointProvider = endpointProvider;
	}

	public async Task<string> GetTokenAsync()
	{
		if (_currentAccessToken.IsNullOrEmpty())
		{
			_currentAccessToken = await GetAccessTokenAsync();
		}

		return _currentAccessToken;
	}

	public async Task<string> GetAccessTokenAsync()
	{
		_logger.LogInformation($"Requesting new access token");

		var client = _factory.CreateClient("ApiGateway");

		var authenticationResponse = await client.PostAsJsonAsync(_endpointProvider.AutheticationService, new
		{
			Username = "Test",
			Password = "Test"
		});

		if (authenticationResponse.IsSuccessStatusCode)
		{
			var jwt = await authenticationResponse.Content.ReadFromJsonAsync<GenerateTokenResponse>();
			return jwt.Token;
		}

		return string.Empty;
	}
}