namespace Ehrms.Web;

internal interface ITokenProvider
{
	Task<string> GetAccessTokenAsync();
}