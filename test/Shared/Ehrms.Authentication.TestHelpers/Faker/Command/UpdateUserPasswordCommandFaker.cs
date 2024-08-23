using System.Reflection.Metadata;

namespace Ehrms.Authentication.TestHelpers.Faker.Command;

public sealed class UpdateUserPasswordCommandFaker : Faker<UpdateUserPasswordCommand>
{
    public UpdateUserPasswordCommandFaker()
    {
        RuleFor(x => x.Username, f => f.Person.UserName);
        RuleFor(x => x.CurrentPassword, f => f.Random.AlphaNumeric(10));
        RuleFor(x => x.NewPassword, f => f.Random.AlphaNumeric(10));
    }

    public UpdateUserPasswordCommandFaker WithUserName(string username)
    {
        RuleFor(x => x.Username, username);
        return this;
    }

    public UpdateUserPasswordCommandFaker WithNewPassword(string newPassword)
    {
        RuleFor(x => x.NewPassword, newPassword);
        return this;
    }
    
    public UpdateUserPasswordCommandFaker WithCurrentPassword(string password)
    {
        RuleFor(x => x.CurrentPassword, password);
        return this;
    }
}