using System.Net;
using Microsoft.AspNetCore.Mvc;
using Ehrms.Authentication.API.Exceptions;
using Ehrms.Authentication.API.Controllers;

namespace Ehrms.Authentication.API.Extension;

internal static class AccountControllerExtensions
{
    internal static IActionResult MapLoginFailureResult(this AccountController controller, Exception err)
    {
        return err switch
        {
            UserAccountInactiveException => controller.StatusCode((int)HttpStatusCode.Forbidden, err.Message),
            UserNotFoundException or UserCredentialsInvalidException => controller.BadRequest(err.Message),
            _ => controller.Unauthorized(err.Message)
        };
    }

    internal static IActionResult MapRefreshFailureResult(this AccountController controller, Exception err)
    {
        if (err is UserAccountInactiveException)
        {
            return controller.StatusCode((int)HttpStatusCode.Forbidden, err.Message);
        }

        if (err is UserNotFoundException)
        {
            return controller.BadRequest(err.Message);
        }

        return controller.Unauthorized(err.Message);
    }
}