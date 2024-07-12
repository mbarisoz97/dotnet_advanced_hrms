namespace Ehrms.Web.Client;

internal static class HttpResponseExtensions
{
    internal static async Task<T?> GetContentAs<T>(this HttpResponseMessage httpResponseMessage)
    {
        try
        {
            return await httpResponseMessage.Content.ReadFromJsonAsync<T>();
        }
        catch
        {
            return default;
        }
    }

    internal static async Task<Response<T>> AsCustomResponse<T>(this HttpResponseMessage httpResponseMessage)
    {
        return new Response<T>()
        {
            StatusCode = httpResponseMessage.StatusCode,
            Content = await httpResponseMessage.GetContentAs<T>()
        };
    }
}