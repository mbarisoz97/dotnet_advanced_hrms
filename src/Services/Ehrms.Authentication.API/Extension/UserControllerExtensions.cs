using System.Net;
using Microsoft.AspNetCore.Mvc;
using Ehrms.Authentication.API.Exceptions;
using Ehrms.Authentication.API.Controllers;

namespace Ehrms.Authentication.API.Extension;

internal static class UserControllerExtensions
{
    internal static IActionResult MapUserUpdateFailureResult(this UserController controller, Exception err)
    {
        if (err is UserUpdateNotAllowedException)
        {
            return controller.Unauthorized();
        }

        return controller.BadRequest();
    }

    internal static IActionResult MapUserResetPasswordFailureResult(this UserController controller, Exception err)
    {
        return err switch
        {
            UserNotFoundException => controller.NotFound(err.Message),
            UserAccountInactiveException => controller.Unauthorized(err.Message),
            UserCredentialsInvalidException or 
                UserPasswordResetFailedException => controller.BadRequest(err.Message),
        
            _ => controller.Problem(statusCode: (int)HttpStatusCode.InternalServerError)
        };
    }
}