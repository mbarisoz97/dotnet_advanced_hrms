using Ehrms.Web.Models;

namespace Ehrms.Web.Client.TrainingApi.Info;

public interface ITrainingServiceClient
{
    Task<Response<Guid>> DeleteTrainingAsync(Guid id);
    Task<Response<TrainingModel>> GetTrainingAsync(Guid id);
    Task<Response<TrainingModel>> UpdateTrainingAsync(TrainingModel training);
    Task<Response<IEnumerable<TrainingModel>>> GetTrainings();
    Task<Response<Guid>> CreateTrainingAsync(TrainingModel training);
}
