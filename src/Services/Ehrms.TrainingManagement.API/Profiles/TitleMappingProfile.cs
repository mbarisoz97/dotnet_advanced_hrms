using Ehrms.Contracts.Title;

namespace Ehrms.TrainingManagement.API.Profiles;

public class TitleMappingProfile : Profile
{
    public TitleMappingProfile()
    {
        CreateMap<TitleCreatedEvent, Title>();
        CreateMap<TitleUpdatedEvent, Title>();
    }
}