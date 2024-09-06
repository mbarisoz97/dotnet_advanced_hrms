namespace Ehrms.Contracts.Title;

public sealed class TitleCreatedEvent
{
    public Guid Id { get; set; }
    public string TitleName { get; set; } = string.Empty;
}