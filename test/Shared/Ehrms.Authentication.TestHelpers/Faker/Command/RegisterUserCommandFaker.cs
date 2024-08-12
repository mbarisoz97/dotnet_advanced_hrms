namespace Ehrms.Authentication.TestHelpers.Faker.Command;

public class RegisterUserCommandFaker : Faker<RegisterUserCommand>
{
    public RegisterUserCommandFaker()
    {
        RuleFor(x => x.Username, f => f.Person.UserName);
        RuleFor(x => x.Email, f => f.Person.Email);
        RuleFor(x => x.Password, "1YourD£faultP@ssword!");
    }

    public RegisterUserCommandFaker WithUserName(string username)
    {
        RuleFor(x => x.Username, username);
        return this;
    }

    public RegisterUserCommandFaker WithEmail(string email)
    {
        RuleFor(x => x.Email, email);
        return this;
    }

    public RegisterUserCommandFaker WithPassword(string password)
    {
        RuleFor(x => x.Password, password);
        return this;
    }
}