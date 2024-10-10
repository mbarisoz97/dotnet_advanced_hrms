using MudBlazor;

namespace Ehrms.Web.Extensions;

internal static class DialogResultExtensions
{
    internal static bool IsOk(this DialogResult? dialogResult)
    {
        return dialogResult != null && !dialogResult.Canceled;
    }
}