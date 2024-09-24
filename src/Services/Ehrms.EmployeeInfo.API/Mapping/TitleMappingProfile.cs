using Ehrms.EmployeeInfo.API.Dtos.Title;
using Ehrms.EmployeeInfo.API.Database.Models;
using Ehrms.EmployeeInfo.API.Handlers.Title.Command;
using Ehrms.Contracts.Title;

namespace Ehrms.EmployeeInfo.API.Mapping;

public class TitleMappingProfile : Profile
{
    public TitleMappingProfile()
    {
        AddCommandToModelMappings();
        AddModalToDtoMappings();
        AddModelToMessageQueueEventMappings();
        AddCommandToMessageQueueEventMappings();
    }

    private void AddCommandToMessageQueueEventMappings()
    {
        CreateMap<DeleteTitleCommand, TitleDeletedEvent>();
    }

    private void AddModelToMessageQueueEventMappings()
    {
        CreateMap<Title, TitleCreatedEvent>()
            .ForMember(dest => dest.Name, src => src.MapFrom(p => p.TitleName));
        CreateMap<Title, TitleUpdatedEvent>()
            .ForMember(dest => dest.Name, src => src.MapFrom(p => p.TitleName));
    }

    private void AddModalToDtoMappings()
    {
        CreateMap<Title, Dtos.Title.ReadTitleDto>();
    }

    private void AddCommandToModelMappings()
    {
        CreateMap<CreateTitleCommand, Title>();
        CreateMap<UpdateTitleCommand, Title>();
    }
}