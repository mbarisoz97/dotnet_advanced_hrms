using System.Net;

namespace Ehrms.Web.Client;

public sealed class Response<T>
{
    public HttpStatusCode StatusCode { get; set; }
    public T? Content { get; set; }
}
