using AutoMapper;
using Ehrms.Administration.API.Database.Models;
using Ehrms.Contracts.Employee;

namespace Ehrms.Administration.API.Profiles;

public class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
        AddMessageQueueEventToModelMappings();
    }

    private void AddMessageQueueEventToModelMappings()
    {
        CreateMap<EmployeeCreatedEvent, Employee>();
        CreateMap<EmployeeUpdatedEvent, Employee>();
    }
}