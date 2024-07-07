using System.Net;
using System.Net.Http.Headers;

namespace Ehrms.Web;

internal class BearerTokenHandler : DelegatingHandler
{
	private readonly ITokenProvider _tokenProvider;

	public BearerTokenHandler(ITokenProvider tokenProvider)
	{
		_tokenProvider = tokenProvider;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var response = await base.SendAsync(request, cancellationToken);
	
		if (response.StatusCode == HttpStatusCode.Unauthorized)
		{
			string refreshedAccessToken = await _tokenProvider.GetAccessTokenAsync();
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", refreshedAccessToken);
		}

		return await base.SendAsync(request, cancellationToken);
	}
}