using Ehrms.Web.Models.Administration;

namespace Ehrms.Web.Client.Administration;

public interface IPaymentCategoryClient
{
    Task<Response<Guid>> DeletePaymentRecordById(Guid id);
    Task<Response<IEnumerable<PaymentCategoryModel>>> GetPaymentCategories();
    Task<Response<PaymentCategoryModel>> GetPaymentCategoryById(Guid id);
    Task<Response<PaymentCategoryModel>> CreatePaymentCategory(PaymentCategoryModel paymentCategory);
    Task<Response<PaymentCategoryModel>> UpdatePaymentCategory(PaymentCategoryModel paymentCategory);
}