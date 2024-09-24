using Ehrms.TrainingManagement.API.Dtos.RecommendationPreference;

namespace Ehrms.TrainingManagement.API.Profiles;

public class TrainingRecommendationPreferenceMappingProfile : Profile
{
    public TrainingRecommendationPreferenceMappingProfile()
    {
        CreateMap<Title, TitleDto>();
        CreateMap<Skill, SkillDto>();
        CreateMap<Project, ProjectDto>();
        CreateMap<TrainingRecommendationPreferences, ReadRecommendationPreferenceDto>();
    }
}