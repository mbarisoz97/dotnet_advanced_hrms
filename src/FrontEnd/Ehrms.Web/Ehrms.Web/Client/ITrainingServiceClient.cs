using Ehrms.Web.Models;

namespace Ehrms.Web.Client;

public interface ITrainingServiceClient
{
	Task<Response<IEnumerable<TrainingModel>>> GetTrainings();
}
