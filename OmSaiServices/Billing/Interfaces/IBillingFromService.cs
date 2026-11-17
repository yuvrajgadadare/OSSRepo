using OmSaiServices.Common;
using OmSaiModels.Billing;

namespace OmSaiServices.Billing.Interfaces
{
	public interface IBillingFromService : IRepository<BillingFromModel>
	{
		List<BillingFromModel> GetAll(int? BillingFromId = null, string? Name = null, string? Email = null, string? Phone1 = null, string? GSTIN = null); BillingFromModel GetById(int id);
	}
}
