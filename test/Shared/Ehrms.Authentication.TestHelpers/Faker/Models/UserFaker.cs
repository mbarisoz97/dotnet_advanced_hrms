﻿using Ehrms.Authentication.API.Database.Models;

namespace Ehrms.Authentication.TestHelpers.Faker.Models;

public class UserFaker : Faker<User>
{
    public UserFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Email, f => f.Person.Email);
        RuleFor(x => x.UserName, f => f.Person.UserName);
        RuleFor(x => x.SecurityStamp, f => f.Random.Guid().ToString());

        RuleFor(x => x.RefreshToken, f => f.Random.AlphaNumeric(20));
        RuleFor(x => x.RefreshTokenExpiry, f => f.Date.Future());
    }

    public UserFaker WithAccountStatus(bool isActive)
    {
        RuleFor(x => x.IsActive, isActive);
        return this;
    }
}