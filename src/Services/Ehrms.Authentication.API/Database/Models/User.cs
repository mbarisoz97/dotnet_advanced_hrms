﻿using Microsoft.AspNetCore.Identity;

namespace Ehrms.Authentication.API.Database.Models;

public sealed class User : IdentityUser<Guid>
{
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiry { get; set; }
}
