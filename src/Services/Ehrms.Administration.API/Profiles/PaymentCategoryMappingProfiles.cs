using AutoMapper;
using Ehrms.Administration.API.Database.Models;
using Ehrms.Administration.API.Dto.PaymentCategorty;
using Ehrms.Administration.API.Handlers.PaymentCategory.Commands;

namespace Ehrms.Administration.API.Profiles;

public class PaymentCategoryMappingProfiles : Profile
{
	public PaymentCategoryMappingProfiles()
	{
		AddModelToDtoMappings();
		AddCommandToModelMappings();
	}

	private void AddCommandToModelMappings()
	{
		CreateMap<CreatePaymentCategoryCommand, PaymentCategory>();
		CreateMap<UpdatePaymentCategoryCommand, PaymentCategory>();
	}

	private void AddModelToDtoMappings()
	{
		CreateMap<PaymentCategory, ReadPaymentCategoryDto>();
	}
}