namespace Ehrms.Web.Models.User;

public sealed class UserRoleModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public override string ToString()
    {
        return Name;
    }
}