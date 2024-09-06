namespace Ehrms.Contracts.Title;

public sealed class TitleUpdatedEvent
{
    public Guid Id { get; set; }
    public string TitleName { get; set; } = string.Empty;
}
