using Ehrms.Web.Validation;
using System.ComponentModel.DataAnnotations;

namespace Ehrms.Web.Models;

public sealed class TrainingModel
{
    public Guid Id { get; set; }

    [Required] [Length(2, 50)] public string? Name { get; set; }

    [Required] public string? Description { get; set; }

    [Required] public DateTime? PlannedAt { get; set; } = DateTime.UtcNow;
    
    [Required] public DateTime? StartsAt { get; set; }

    [Required]
    [FutureDateRequired(nameof(StartsAt))]
    public DateTime? EndsAt { get; set; }

    public ICollection<Guid> Participants { get; set; } = [];
}