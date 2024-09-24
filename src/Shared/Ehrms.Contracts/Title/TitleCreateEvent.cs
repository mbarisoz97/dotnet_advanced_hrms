namespace Ehrms.Contracts.Title;

public sealed class TitleCreatedEvent
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}