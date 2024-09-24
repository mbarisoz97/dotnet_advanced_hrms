namespace Ehrms.Web.Client.TrainingApi.Preference;

internal interface ITrainingPreferenceClient
{
    Task<Response<Guid>> DeleteTrainingPreferenceByIdAsync(Guid id);
    Task<Response<IEnumerable<TrainingPreferenceModel>>> GetTrainingPreferenceAsync();
    Task<Response<TrainingPreferenceModel>> GetTrainingPreferenceByIdAsync(Guid id);
    Task<Response<TrainingPreferenceModel>> UpdateTrainingPreferenceAsync(UpdateTrainingPreferenceModel updateTrainingPreferenceModel);
    Task<Response<TrainingPreferenceModel>> CreateTrainingPreferenceAsync(CreateTrainingPreferenceModel createTrainingPreference);
}