using Microsoft.Data.SqlClient;
using OmSaiModels.Invoice;
using OmSaiServices.Billing.Implementations;
using OmSaiServices.Common;
using OmSaiServices.Invoice.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace OmSaiServices.Invoice.Implementations
{
	public class InvoiceItemService : Repository<InvoiceItemModel>, IInvoiceItemService
	{
		private readonly string usp_cud;
		private readonly string usp_r;
		private readonly Mapper _mapper;

		public InvoiceItemService()
		{
			usp_cud = "usp_CreateUpdateDelete_InvoiceItems";
			usp_r = "usp_GetAll_InvoiceItems";
			_mapper = new Mapper();
		}

		public int Create(InvoiceItemModel model)
		{
			// Validate qty
			if (model.Qty <= 0)
				throw new ArgumentException("Quantity must be positive");
			if (model.CurrentPrice < 0)
				throw new ArgumentException("Price cannot be negative");

			return Create(model, usp_cud, CreateUpdate(model, "INSERT"));
		}

		public void Update(InvoiceItemModel model)
		{
			Update(model, usp_cud, CreateUpdate(model, "UPDATE"));
		}

		public void Delete(int id)
		{
			Delete(usp_cud, DeleteParams(id));
		}

		private List<KeyValuePair<string, object>> DeleteParams(int id)
		{
			return new List<KeyValuePair<string, object>>
			{
				new("@InvoiceItemId", id)
			};
		}

		public List<InvoiceItemModel> GetAll(int? InvoiceItemId = null, int? InvoiceId = null)
		{
			var mapEntity = new Func<IDataReader, InvoiceItemModel>(reader => _mapper.MapEntity<InvoiceItemModel>(reader));

			return GetAll(usp_r, mapEntity, GetParams(InvoiceItemId, InvoiceId));
		}

		public InvoiceItemModel GetById(int id)
		{
			var mapEntity = new Func<IDataReader, InvoiceItemModel>(reader => _mapper.MapEntity<InvoiceItemModel>(reader));

			return GetById(id, usp_r, mapEntity, GetParams(id));
		}

		private List<KeyValuePair<string, object>> CreateUpdate(InvoiceItemModel model, string type)
		{
			return new List<KeyValuePair<string, object>>
			{
				new("@InvoiceItemId", type == "INSERT" ? 0 : model.InvoiceItemId),
				new("@InvoiceId", model.InvoiceId),
				new("@ProductsServiceId", model.ProductsServiceId),
				new("@Description", model.Description ?? (object)DBNull.Value),
				new("@HsnSac", model.HsnSac ?? (object)DBNull.Value),
				new("@Qty", model.Qty),
				new("@PresentDays", model.PresentDays),
				
				new("@CurrentPrice", model.CurrentPrice),
				new("@Unit", model.Unit ?? (object)DBNull.Value),

				new("@CGSTPercentage", model.CGSTPercentage),
				new("@SGSTPercentage", model.SGSTPercentage),
				new("@IGSTPercentage", model.IGSTPercentage),

				new("@CGSTAmount", model.CGSTAmount),
				new("@SGSTAmount", model.SGSTAmount),
				new("@IGSTAmount", model.IGSTAmount),
				new("@IsActive", model.IsActive),
				new("@UpdatedIP", model.UpdatedIP ?? (object)DBNull.Value)
			};
		}

		private SqlParameter[] GetParams(int? InvoiceItemId = null, int? InvoiceId = null)
		{
			return new SqlParameter[]
			{
				new SqlParameter("@InvoiceItemId", InvoiceItemId ?? (object)DBNull.Value),
				new SqlParameter("@InvoiceId", InvoiceId ?? (object)DBNull.Value)
			};
		}
	}
}
