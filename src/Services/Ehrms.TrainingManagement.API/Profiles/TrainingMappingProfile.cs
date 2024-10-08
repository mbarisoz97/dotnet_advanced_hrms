﻿namespace Ehrms.TrainingManagement.API.Profiles;

public class TrainingMappingProfile : Profile
{
	public TrainingMappingProfile()
	{
		AddModelToDtoMappings();
		AddCommandToModelMappings();
	}

	private void AddCommandToModelMappings()
	{
		CreateMap<CreateTrainingCommand, Training>()
			.ForMember(dest => dest.Participants, opt => opt.Ignore());

		CreateMap<UpdateTrainingCommand, Training>()
			.ForMember(dest => dest.Participants, opt => opt.Ignore());
	}

	private void AddModelToDtoMappings()
	{
		CreateMap<Training, ReadTrainingDto>()
			.ForMember(dest => dest.Participants,
				opt => opt.MapFrom(src => src.Participants.Select(x => x.Id)));
	}
}