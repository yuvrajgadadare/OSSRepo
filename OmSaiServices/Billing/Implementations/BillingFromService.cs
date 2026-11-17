using Microsoft.Data.SqlClient;
using OmSaiModels.Billing;
using OmSaiModels.Worker;
using OmSaiServices.Billing.Interfaces;
using OmSaiServices.Common;
using System.Data;

namespace OmSaiServices.Billing.Implementations
{
	public class BillingFromService : Repository<BillingFromModel>, IBillingFromService
	{
		private readonly string usp_cud;
		private readonly string usp_r;
		private readonly Mapper _mapper;

		public BillingFromService()
		{
			usp_cud = "usp_CreateUpdateDeleteRestore_BillingFroms";
			usp_r = "usp_GetAll_BillingFroms";
			_mapper = new Mapper();
		}

		public int Create(BillingFromModel model)
		{
			return Create(model, usp_cud, CreateUpdate(model, "create"));
		}

		public void Update(BillingFromModel model)
		{
			Update(model, usp_cud, CreateUpdate(model, "update"));
		}


		public void Delete(int id)
		{
			Delete(usp_cud, DeleteRestore(id));
		}

		public void Restore(int id)
		{
			Restore(usp_cud, DeleteRestore(id));
		}

		private List<KeyValuePair<string, object>> DeleteRestore(int id)
		{

			return new List<KeyValuePair<string, object>>
			{
				new("@BillingFromId", id),

			};

		}

		public List<BillingFromModel> GetAll(int? BillingFromId = null, string? Name = null, string? Email = null, string? Phone1 = null, string? GSTIN = null)
		{

			// Define the mapping function
			var mapEntity = new Func<IDataReader, BillingFromModel>(reader => _mapper.MapEntity<BillingFromModel>(reader));

			return GetAll(usp_r, mapEntity, GetParams(BillingFromId,Name,Email,Phone1,GSTIN));
		}

		public BillingFromModel GetById(int id)
		{
			// Define the mapping function
			var mapEntity = new Func<IDataReader, BillingFromModel>(reader => _mapper.MapEntity<BillingFromModel>(reader));

			return GetById(id, usp_r, mapEntity, GetParams(id));
		}


		private List<KeyValuePair<string, object>> CreateUpdate(BillingFromModel model, string type)
		{
			var BillingFromId = type == "create" ? 0 : model.BillingFromId;

   
			return new List<KeyValuePair<string, object>>
			{
				new("@BillingFromId", BillingFromId),
				new("@Name", model.Name),
				new("@Email", model.Email),
				new("@Phone1", model.Phone1),
				new("@Phone2", model.Phone2 ?? (object)DBNull.Value),
				new("@GSTIN", model.GSTIN),
				new("@StateName", model.StateName??"Maharashtra"),
				new("@StateCode", model.StateCode??27),
				new("@Address", model.Address),
				new("@BankAccountOwnerName", model.BankAccountOwnerName),
				new("@BankName", model.BankName),
				new("@BankBranchName", model.BankBranchName),
				new("@BankAccountNumber", model.BankAccountNumber),
				new("@BankIFSCCode", model.BankIFSCCode),
			//	new("@IsActive", model.IsActive),
				new("@UpdatedIP", model.UpdatedIP ?? (object)DBNull.Value),
			};
		}


		private SqlParameter[] GetParams(int? BillingFromId = null, string Name = null, string Email = null, string Phone1 = null, string GSTIN = null)
		{
			return new SqlParameter[]
			{
		new SqlParameter("@BillingFromId", BillingFromId ?? (object)DBNull.Value),
		new SqlParameter("@Name", Name ?? (object)DBNull.Value),
		new SqlParameter("@Email", Email ?? (object)DBNull.Value),
		new SqlParameter("@Phone1", Phone1 ?? (object)DBNull.Value),
		new SqlParameter("@GSTIN", GSTIN ?? (object)DBNull.Value),
			};
		}



	}
}
