using Microsoft.Data.SqlClient;
using OmSaiModels.Admin;
using OmSaiModels.Worker;
using OmSaiEnvironment;
using OmSaiServices.Common;
using OmSaiServices.Worker.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using LmsServices.Common;
using System.Reflection;

namespace OmSaiServices.Worker.Implementations
{
    public class WorkerSalaryService : Repository<SalaryProjectSitesModel>, IWorkerSalaryService
    {
        private readonly string sp_cud;  // Stored Procedure for Insert, Update, Delete, Restore
        private readonly string sp_r;    // Stored Procedure for GetAll
        private readonly string sp_r_monthly_salary;    // Stored Procedure for GetAll
        private readonly string sp_cu_monthly_salary;  // Stored Procedure for Insert, Update, Delete, Restore
        private readonly Mapper _mapper;

        public WorkerSalaryService()
        {
            sp_cud = "usp_CreateUpdateDeleteRestore_WorkerSalaries";
            sp_r = "usp_GetAll_WorkerSalaries";

			sp_r_monthly_salary = "usp_GetAll_WorkerMonthlySalaries";
			sp_cu_monthly_salary = "usp_InsertOrUpdate_WorkerMonthlySalaries";

			_mapper = new Mapper();
        }

		public void UpdateMonthlySalary(int year, int month, int? workerId = null)
		{
			var parameters=  new List<KeyValuePair<string, object>>
			{
				new("@Year", year),
				new("@Month", month),
				new("@WorkerId", workerId)
			};

			QueryService.NonQuery(sp_cu_monthly_salary, parameters);
		}


		public int Create(SalaryProjectSitesModel model)
        {
            return Create(model, sp_cud, CreateUpdate(model, "INSERT"));
        }

        public void Update(SalaryProjectSitesModel model)
        {
            Update(model, sp_cud, CreateUpdate(model, "UPDATE"));
        }

        public void Delete(int id)
        {
            Delete(sp_cud, DeleteRestore(id, "DELETE"));
        }

        public void Restore(int id)
        {
            Restore(sp_cud, DeleteRestore(id, "RESTORE"));
        }


		public List<SalaryProjectSitesModel> GetAll()
        {
            var mapEntity = new Func<IDataReader, SalaryProjectSitesModel>(reader => _mapper.MapEntity<SalaryProjectSitesModel>(reader));
            return GetAll(sp_r, mapEntity, GetParams());
        }
        public List<SalaryProjectSitesModel> GetByWorkerId(int id)
        {

            // Define the mapping function
            var mapEntity = new Func<IDataReader, SalaryProjectSitesModel>(reader => _mapper.MapEntity<SalaryProjectSitesModel>(reader));

            return GetAll(sp_r, mapEntity, GetParams(null, id));
        }
        public List<SalaryProjectSitesModel> GetBySiteId(int SiteId)
        {

            // Define the mapping function
            var mapEntity = new Func<IDataReader, SalaryProjectSitesModel>(reader => _mapper.MapEntity<SalaryProjectSitesModel>(reader));

            return GetAll(sp_r, mapEntity, GetParams(null, null, SiteId));
        }

        public SalaryProjectSitesModel GetById(int id)
        {
            var mapEntity = new Func<IDataReader, SalaryProjectSitesModel>(reader => _mapper.MapEntity<SalaryProjectSitesModel>(reader));
            return GetById(id, sp_r, mapEntity, GetParams(id));
        }

        private List<KeyValuePair<string, object>> CreateUpdate(SalaryProjectSitesModel model, string type)
        {
            var WorkerSalaryId = type == "INSERT" ? 0 : model.WorkerSalaryId;
            return new List<KeyValuePair<string, object>>
            {
                new("@WorkerSalaryId", WorkerSalaryId),
                new("@WorkerId", model.WorkerId),
                new("@MonthlySalary", model.MonthlySalary),			
				new("@ShiftHrs", model.ShiftHrs),
				new("@HourlyWage", model.HourlyWage),

                new("@DAAmount", model.DAAmount),
                new("@EPFAmount", model.EPFAmount),
                new("@ESICAmount", model.ESICAmount),
                new("@PTAmount", model.PTAmount),

			};
        }

        private List<KeyValuePair<string, object>> DeleteRestore(int id, string type)
        {
            return new List<KeyValuePair<string, object>>
            {

                new("@WorkerSalaryId", id)
            };
        }

        private SqlParameter[] GetParams(int? id = null, int? WorkerId = null, int? SiteId = null, DateTime? SalaryMonth = null)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@WorkerSalaryId", id ?? (object)DBNull.Value),
                new SqlParameter("@WorkerId", WorkerId ?? (object)DBNull.Value),
                 new SqlParameter("@SiteId", SiteId ?? (object)DBNull.Value)
            };
        }

        private void ExecuteNonQuery(string storedProcedure, List<KeyValuePair<string, object>> parameters)
        {
            using (var connection = new SqlConnection(DBConnection.DefaultConnection))
            {
                using (var command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }




}
