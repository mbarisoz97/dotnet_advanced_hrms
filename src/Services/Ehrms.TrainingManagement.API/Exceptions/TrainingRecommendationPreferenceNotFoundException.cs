namespace Ehrms.TrainingManagement.API.Exceptions;

internal sealed class TrainingRecommendationPreferenceNotFoundException : CustomNotFoundException
{
    public TrainingRecommendationPreferenceNotFoundException(string message) : base(message)
    {
    }
}