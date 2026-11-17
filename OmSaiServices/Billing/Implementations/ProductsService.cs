using Microsoft.Data.SqlClient;
using OmSaiModels.Billing;
using OmSaiServices.Billing.Interfaces;
using OmSaiServices.Common;
using System;
using System.Collections.Generic;
using System.Data;

namespace OmSaiServices.Billing.Implementations
{
	public class ProductsService : Repository<ProductsServiceModel>, IProductsService
	{
		private readonly string usp_cud;
		private readonly string usp_r;
		private readonly Mapper _mapper;

		public ProductsService()
		{
			usp_cud = "usp_CreateUpdateDeleteRestore_ProductsServices";
			usp_r = "usp_GetAll_ProductsServices";
			_mapper = new Mapper();
		}

		public int Create(ProductsServiceModel model)
		{
			return Create(model, usp_cud, CreateUpdate(model, "INSERT"));
		}

		public void Update(ProductsServiceModel model)
		{
			Update(model, usp_cud, CreateUpdate(model, "UPDATE"));
		}

		public void Delete(int id)
		{
			Delete(usp_cud, DeleteRestore(id, "DELETE"));
		}

		public void Restore(int id)
		{
			Restore(usp_cud, DeleteRestore(id, "RESTORE"));
		}

		private List<KeyValuePair<string, object>> DeleteRestore(int id, string type)
		{
			return new List<KeyValuePair<string, object>>
			{
				new("@ProductsServiceId", id)
			};
		}

		public List<ProductsServiceModel> GetAll(int? ProductsServiceId = null, string ItemType = null, string Name = null, string HSNSACCode = null, string TaxCategory = null)
		{
			var mapEntity = new Func<IDataReader, ProductsServiceModel>(reader => _mapper.MapEntity<ProductsServiceModel>(reader));

			return GetAll(usp_r, mapEntity, GetParams(ProductsServiceId, ItemType, Name, HSNSACCode, TaxCategory));
		}

		public ProductsServiceModel GetById(int id)
		{
			var mapEntity = new Func<IDataReader, ProductsServiceModel>(reader => _mapper.MapEntity<ProductsServiceModel>(reader));

			return GetById(id, usp_r, mapEntity, GetParams(id));
		}

		private List<KeyValuePair<string, object>> CreateUpdate(ProductsServiceModel model, string type)
		{
			var ProductsServiceId = type == "INSERT" ? 0 : model.ProductsServiceId;

			return new List<KeyValuePair<string, object>>
			{
				new("@ProductsServiceId", ProductsServiceId),
				new("@ItemType", model.ItemType),
				new("@Name", model.Name),
				new("@Description", model.Description ?? (object)DBNull.Value),
				new("@Unit", model.Unit),
				new("@Price", model.Price),
				new("@HSNSACCode", model.HSNSACCode ?? (object)DBNull.Value),
				new("@TaxCategory", model.TaxCategory ?? (object)DBNull.Value),
				new("@TaxPercentage", model.TaxPercentage ?? (object)DBNull.Value),
				new("@CGST", model.CGST ?? (object)DBNull.Value),
				new("@SGST", model.SGST ?? (object)DBNull.Value),
				new("@IGST", model.IGST ?? (object)DBNull.Value),
				new("@DiscountPercentage", model.DiscountPercentage ?? (object)DBNull.Value),
				new("@FinalPrice", model.FinalPrice ?? (object)DBNull.Value),
				new("@TaxInclusive", model.TaxInclusive),
				new("@UpdatedIP", model.UpdatedIP ?? (object)DBNull.Value)
			};
		}

		private SqlParameter[] GetParams(int? ProductsServiceId = null, string ItemType = null, string Name = null, string HSNSACCode = null, string TaxCategory = null)
		{
			return new SqlParameter[]
			{
				new SqlParameter("@ProductsServiceId", ProductsServiceId ?? (object)DBNull.Value),
				new SqlParameter("@ItemType", ItemType ?? (object)DBNull.Value),
				new SqlParameter("@Name", Name ?? (object)DBNull.Value),
				new SqlParameter("@HSNSACCode", HSNSACCode ?? (object)DBNull.Value),
				new SqlParameter("@TaxCategory", TaxCategory ?? (object)DBNull.Value)
			};
		}
	}
}
