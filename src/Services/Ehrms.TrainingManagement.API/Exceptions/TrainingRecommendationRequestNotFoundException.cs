namespace Ehrms.TrainingManagement.API.Exceptions;

internal sealed class TrainingRecommendationRequestNotFoundException : CustomNotFoundException
{
    public TrainingRecommendationRequestNotFoundException(string message) : base(message)
    {
    }
}