namespace Ehrms.Contracts.Title;

public sealed class TitleCreateEvent
{
    public Guid Id { get; set; }
    public string TitleName { get; set; } = string.Empty;
}