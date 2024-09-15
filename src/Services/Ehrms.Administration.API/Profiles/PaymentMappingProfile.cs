using AutoMapper;
using Ehrms.Administration.API.Database.Models;
using Ehrms.Administration.API.Dto.Payment;
using Ehrms.Administration.API.Handlers.Payment.Commands;

namespace Ehrms.Administration.API.Profiles;

public class PaymentMappingProfile : Profile
{
	public PaymentMappingProfile()
	{
		AddModelToDtoMappings();
		AddCommandToModelMappings();
	}

	private void AddCommandToModelMappings()
	{
		CreateMap<UpdatePaymentCommand, PaymentCriteria>();
		CreateMap<CreatePaymentCommand, PaymentCriteria>();
	}

	private void AddModelToDtoMappings()
	{
		CreateMap<PaymentCriteria, ReadPaymentDto>()
			.ForMember(dest => dest.EmployeeId,
				opt => opt.MapFrom(src => src!.Employee!.Id))
			.ForMember(dest => dest.PaymentCategoryId,
				opt => opt.MapFrom(src => src!.PaymentCategory!.Id));
	}
}
