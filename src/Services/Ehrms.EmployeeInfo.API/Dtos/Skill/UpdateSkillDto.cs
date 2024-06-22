namespace Ehrms.EmployeeInfo.API.Dtos.Skill;

public sealed class UpdateSkillDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}