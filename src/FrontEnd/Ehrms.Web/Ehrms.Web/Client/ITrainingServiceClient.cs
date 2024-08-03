using Ehrms.Web.Models;

namespace Ehrms.Web.Client;

public interface ITrainingServiceClient
{
	Task<Response<Guid>> DeleteTrainingAsync(Guid id);
	Task<Response<TrainingModel>> GetTrainingAsync(Guid id);
	Task<Response<TrainingModel>> UpdateTrainingAsync(TrainingModel training);
	Task<Response<IEnumerable<TrainingModel>>> GetTrainings();
	Task<Response<Guid>> CreateTrainingAsync(TrainingModel training);
    Task<Response<Guid>> CreateTrainingRecommendationRequest(CreateTrainingRecommendationRequestModel createTrainingRecommendationRequest);
    Task<Response<IEnumerable<ReadTrainingRecommendationRequestModel>>> GetTrainingRecommendationRequests();
}
