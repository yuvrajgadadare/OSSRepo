using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.SqlClient;
using OmSaiModels.Billing;
using OmSaiServices.Billing.Interfaces;
using OmSaiServices.Common;
using System;
using System.Collections.Generic;
using System.Data;

namespace OmSaiServices.Billing.Implementations
{
	public class BillingCustomerService : Repository<BillingCustomerModel>, IBillingCustomerService
	{
		private readonly string usp_cud;
		private readonly string usp_r;
		private readonly Mapper _mapper;

		public BillingCustomerService()
		{
			usp_cud = "usp_CreateUpdateDeleteRestore_BillingCustomers";
			usp_r = "usp_GetAll_BillingCustomers";
			_mapper = new Mapper();
		}

		public int Create(BillingCustomerModel model)
		{
			return Create(model, usp_cud, CreateUpdate(model, "INSERT"));
		}

		public void Update(BillingCustomerModel model)
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
				new("@BillingCustomerId", id)
			};
		}

		public List<BillingCustomerModel> GetAll(int? BillingCustomerId = null, string Name = null, string Email = null, string Phone1 = null, string Phone2 = null, string GSTIN = null)
		{
			var mapEntity = new Func<IDataReader, BillingCustomerModel>(reader => _mapper.MapEntity<BillingCustomerModel>(reader));

			return GetAll(usp_r, mapEntity, GetParams(BillingCustomerId, Name, Email, Phone1, Phone2, GSTIN));
		}

		public BillingCustomerModel GetById(int id)
		{
			var mapEntity = new Func<IDataReader, BillingCustomerModel>(reader => _mapper.MapEntity<BillingCustomerModel>(reader));

			return GetById(id, usp_r, mapEntity, GetParams(id));
		}

		private List<KeyValuePair<string, object>> CreateUpdate(BillingCustomerModel model, string type)
		{
			var BillingCustomerId = type == "INSERT" ? 0 : model.BillingCustomerId;



			return new List<KeyValuePair<string, object>>
			{
				new("@BillingCustomerId", BillingCustomerId),
				new("@Name", model.Name),
				new("@Email", model.Email),
				new("@Phone1", model.Phone1),
				new("@Phone2", model.Phone2 ?? (object)DBNull.Value),
				new("@Address", model.Address),
				new("@GSTIN", model.GSTIN),
				new("@StateName", model.StateName??"Maharashtra"),
				new("@StateCode", model.StateCode??27),
				new("@UpdatedIP", model.UpdatedIP ?? (object)DBNull.Value)
			};
		}

		private SqlParameter[] GetParams(int? BillingCustomerId = null, string Name = null, string Email = null, string Phone1 = null, string Phone2 = null, string GSTIN = null)
		{
			return new SqlParameter[]
			{
				new SqlParameter("@BillingCustomerId", BillingCustomerId ?? (object)DBNull.Value),
				new SqlParameter("@Name", Name ?? (object)DBNull.Value),
				new SqlParameter("@Email", Email ?? (object)DBNull.Value),
				new SqlParameter("@Phone1", Phone1 ?? (object)DBNull.Value),
				new SqlParameter("@Phone2", Phone2 ?? (object)DBNull.Value),
				new SqlParameter("@GSTIN", GSTIN ?? (object)DBNull.Value)
			};
		}
	}
}
