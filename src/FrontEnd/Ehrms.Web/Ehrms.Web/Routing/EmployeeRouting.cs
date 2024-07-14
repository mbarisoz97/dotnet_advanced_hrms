﻿namespace Ehrms.Web;

internal static class EmployeeRouting
{
	internal const string Index = "/Employee";
	internal const string Create = $"{Index}/Create";
	internal const string Update = $"{Index}/Update";
	internal const string Delete = $"{Index}/Delete";
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
	internal const string Login = "/Login";
	internal const string Logout = "/Logout";
}

internal static class ProjectRouting
{
	internal const string Index = "/Project";
	internal const string Delete = $"/{Index}/Delete";
}