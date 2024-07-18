namespace Ehrms.Contracts.Skill;

public sealed class SkillUpdatedEvent
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
}
