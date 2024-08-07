namespace Ehrms.TrainingManagement.API.Profiles;

public class RecommendationResultMappingProfile : Profile
{
    public RecommendationResultMappingProfile()
    {
        CreateMap<TrainingRecommendationResult, ReadTrainingRecommendationResultDto>()
            .ForMember(dest => dest.Skill, opt => opt.MapFrom(src => src.Skill.Name))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.RecommendationRequest.Title));
    }
}