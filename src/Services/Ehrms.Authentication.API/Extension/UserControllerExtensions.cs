using Ehrms.Authentication.API.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Ehrms.Authentication.API.Controllers;

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
}