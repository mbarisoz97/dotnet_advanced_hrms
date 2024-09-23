namespace Ehrms.Web.Models.Training;

public class UpdateTrainingPreferenceModel
{
    public Guid Id { get; set; }
    public ICollection<Guid> Skills { get; set; } = [];
}
