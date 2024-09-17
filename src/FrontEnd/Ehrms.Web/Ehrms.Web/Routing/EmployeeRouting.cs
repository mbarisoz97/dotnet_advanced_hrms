namespace Ehrms.Web.Routing;

internal partial class PageRouting
{
    internal class Employee
    {
        internal const string Index = "/Employee";
    }

    internal class Title
    {
        internal const string Index = "/Title";
    }

    internal class Skill
    {
        internal const string Index = "/Skill";
    }

    internal class Training
    {
        internal const string Index = "/Training";
    }

    internal class Project
    {
        internal const string Index = "/Project";
    }

    internal class User
    {
        internal const string Index = "/User";
        internal const string Login = "/Login";
        internal const string Logout = "/Logout";
        internal const string Authenticate = "/Auth";
        internal const string PasswordReset = "/PasswordReset";
    }
}

internal static class EmployeeRouting
{
    internal const string Index = "/Employee";
    internal const string Create = $"{Index}/Create";
    internal const string Update = $"{Index}/Update";
    internal const string Delete = $"{Index}/Delete";
    internal const string Details = $"{Index}/Details";
}

internal static class SkillRouting
{
    internal const string Index = "/Skill";
    internal const string Create = $"{Index}/Create";
    internal const string Update = $"{Index}/Update";
    internal const string Delete = $"{Index}/Delete";
}

internal static class UserRouting
{
    internal const string Index = "/User";
    internal const string Login = "/Login";
    internal const string Logout = "/Logout";
    internal const string Authenticate = "/Auth";
    internal const string PasswordReset = "/PasswordReset";
}

internal static class ProjectRouting
{
    internal const string Index = "/Project";
    internal const string Delete = $"{Index}/Delete";
    internal const string Details = $"{Index}/Details";
}

internal static class TrainingRouting
{
    internal const string Index = "/Training";
    internal const string Recommendation = $"{Index}/Recommendation";
}

internal static class ErrorRouting
{
    internal const string AccessError = "/AccessError";
}