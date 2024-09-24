namespace Ehrms.Web.Client.TrainingApi.Recommendation;

internal interface ITrainingRecommendationServiceClient
{
    Task<Response<Guid>> DeleteTrainingRecommendationRequest(Guid id);
    Task<Response<ReadTrainingRecommendationResultModel>> GetRecommendationResult(Guid id);
    Task<Response<IEnumerable<ReadTrainingRecommendationResultModel>>> GetRecommendationResults(Guid id);
    Task<Response<ReadTrainingRecommendationRequestModel>> GetTrainingRecommendationRequest(Guid id);
    Task<Response<IEnumerable<ReadTrainingRecommendationRequestModel>>> GetTrainingRecommendationRequests();
    Task<Response<Guid>> CreateTrainingRecommendationRequest(CreateTrainingRecommendationRequestModel createTrainingRecommendationRequest);
}