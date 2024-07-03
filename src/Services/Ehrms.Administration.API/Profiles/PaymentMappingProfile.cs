using AutoMapper;
using Ehrms.Administration.API.Handlers.Payment.Commands;

namespace Ehrms.Administration.API.Profiles;

public class PaymentMappingProfile : Profile
{
    public PaymentMappingProfile()
	{
		AddCommandToModelMappings();
	}

	private void AddCommandToModelMappings()
	{
		CreateMap<CreatePaymentCommand, PaymentCriteria>();
	}
}
