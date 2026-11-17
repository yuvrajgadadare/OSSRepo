using OmSaiModels.Billing;
using OmSaiServices.Common;
using System.Collections.Generic;

namespace OmSaiServices.Billing.Interfaces
{
	public interface IProductsService : IRepository<ProductsServiceModel>
	{
		List<ProductsServiceModel> GetAll(int? ProductsServiceId = null, string ItemType = null, string Name = null, string HSNSACCode = null, string TaxCategory = null);
		ProductsServiceModel GetById(int id);
	}
}
