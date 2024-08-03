namespace Ehrms.TrainingManagement.API.Profiles;

public class TrainingRecommendationMappingProfile : Profile
{
    public TrainingRecommendationMappingProfile()
    {
        AddModelToDtoMappings();
        AddModelToMessageQueueEventMappings();
    }
	
    private void AddModelToMessageQueueEventMappings()
    {
        CreateMap<TrainingRecommendationRequest, TrainingRecommendationRequestAcceptedEvent >()
            .ForMember(dest => dest.RequestId, opt => opt.MapFrom(src=>src.Id))
            .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId));
    }

    private void AddModelToDtoMappings()
    {
        CreateMap<TrainingRecommendationRequest, ReadTrainingRequestDto>();
    }
}