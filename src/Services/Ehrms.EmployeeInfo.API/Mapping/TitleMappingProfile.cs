using Ehrms.EmployeeInfo.API.Dtos.Title;
using Ehrms.EmployeeInfo.API.Database.Models;
using Ehrms.EmployeeInfo.API.Handlers.Title.Command;

namespace Ehrms.EmployeeInfo.API.Mapping;

public class TitleMappingProfile : Profile
{
    public TitleMappingProfile()
    {
        AddCommandToModelMappings();
        AddModalToDtoMappings();

    }

    private void AddModalToDtoMappings()
    {
        CreateMap<Title, ReadTitleDto>();
    }

    private void AddCommandToModelMappings()
    {
        CreateMap<CreateTitleCommand, Title>();
        CreateMap<UpdateTitleCommand, Title>();
    }
}