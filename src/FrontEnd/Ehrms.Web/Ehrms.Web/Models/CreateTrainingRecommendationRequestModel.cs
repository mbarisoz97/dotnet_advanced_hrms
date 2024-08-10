using System.ComponentModel.DataAnnotations;

namespace Ehrms.Web.Models;

public sealed class CreateTrainingRecommendationRequestModel
{
    [Required] [Length(2, 50)] public string? Title { get; set; }

    [Required] public Guid ProjectId { get; set; }
}