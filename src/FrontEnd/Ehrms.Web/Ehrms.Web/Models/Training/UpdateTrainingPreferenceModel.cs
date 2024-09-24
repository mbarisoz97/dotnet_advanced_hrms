namespace Ehrms.Web.Models.Training;

public class UpdateTrainingPreferenceModel
{
    public Guid Id { get; set; }
    public IEnumerable<Guid> Skills { get; set; } = [];
}
