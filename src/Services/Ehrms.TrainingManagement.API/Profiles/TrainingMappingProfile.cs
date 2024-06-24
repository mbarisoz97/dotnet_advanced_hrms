namespace Ehrms.TrainingManagement.API.Profiles;

public class TrainingMappingProfile  : Profile
{
    public TrainingMappingProfile()
    {
        AddModelToDtoMappings();
		AddDtoToCommandMappings();
		AddCommandToModelMappings();
    }

    private void AddCommandToModelMappings()
    {
        CreateMap<CreateTrainingCommand, Training>()
            .ForMember(dest => dest.Participants, opt => opt.Ignore());
    }

    private void AddDtoToCommandMappings()
    {
        CreateMap<CreateTrainingDto, CreateTrainingCommand>();
    }

    private void AddModelToDtoMappings()
    {
        CreateMap<Training, ReadTrainingDto>();
    }

}