using LmsServices.Common;
using Microsoft.Data.SqlClient;
using OmSaiModels.Worker;
using OmSaiServices.Worker.Interfaces;
using System.Data;


namespace OmSaiServices.Worker.Implementations
{
	public class WorkerMonthlySalaryService 
	{
		private readonly string sp_r;    // Stored Procedure for GetAll
		private readonly Mapper _mapper;


		public WorkerMonthlySalaryService()
		{
			sp_r = "usp_GetAll_WorkerMonthlySalaries";
			_mapper = new Mapper();
		}


		//WorkerMonthlySalaryEdit

		public void WorkerMonthlySalaryUpdate(WorkerMonthlySalaryEdit model)
		{

			var parameters=  new List<KeyValuePair<string, object>>
			{
				new("@WorkerMonthlySalaryId", model.WorkerMonthlySalaryId),
				new("@AdvanceAmount", model.AdvanceAmount),
				new("@OtherAllowances", model.OtherAllowances),
				new("@OtherAllowancesRemark", model.OtherAllowancesRemark),
				new("@DeductionAmount", model.DeductionAmount),
				new("@DeductionRemark", model.DeductionRemark),

				new("@TotalDeduction", model.TotalDeduction),
				new("@NetSalary", model.NetSalary),
				new("@PaidAmount", model.PaidAmount)
			};

			//string outputParameterName = "@LastInsertedId";
			// Add a @type parameter manually
			QueryService.NonQuery("usp_Update_WorkerMonthlySalaryById", parameters);
		}


		public List<WorkerMonthlySalary> GetAll( int SiteId, int SalaryMonth, int SalaryYear, int? WorkerId = null)
		{
			var mapEntity = new Func<IDataReader, WorkerMonthlySalary>(reader => _mapper.MapEntity<WorkerMonthlySalary>(reader));

			return QueryService.Query(sp_r, mapEntity, GetParams(null, WorkerId, SiteId, SalaryMonth, SalaryYear));
		}

		public WorkerMonthlySalary GetById(int id)
		{
			var mapEntity = new Func<IDataReader, WorkerMonthlySalary>(reader => _mapper.MapEntity<WorkerMonthlySalary>(reader));

			var result = QueryService.Query(sp_r, mapEntity, GetParams(id));
			return result?.FirstOrDefault();
		}


		private SqlParameter[] GetParams(int? id = null, int? WorkerId = null, int? SiteId = null, int? SalaryMonth = null, int? SalaryYear = null)
		{
			return new SqlParameter[]
			{
				new SqlParameter("@WorkerMonthlySalaryId", id ?? (object)DBNull.Value),
				new SqlParameter("@WorkerId", WorkerId ?? (object)DBNull.Value),
				 new SqlParameter("@SiteId", SiteId ?? (object)DBNull.Value),
				 new SqlParameter("@Month", SalaryMonth ?? (object)DBNull.Value),
				 new SqlParameter("@Year", SalaryYear ?? (object)DBNull.Value)
			};
		}


	}
}
