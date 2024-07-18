namespace Ehrms.Contracts.Skill;

public sealed class SkillCreatedEvent
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;	
}
