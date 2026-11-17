using Microsoft.Data.SqlClient;
using OmSaiModels.Dashboard;
using OmSaiServices.Common;
using OmSaiServices.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Reports.Implementations
{

	public class DailyAttendanceSummaryService : Repository<DailyAttendanceSummary>, IDailyAttendanceSummaryService
	{
		private readonly string sp_cud;     // sp_CreateUpdateDeleteRestore_DailyAttendanceSummaries
		private readonly string sp_r;       // sp_GetAll_DailyAttendanceSummaries
		private readonly Mapper _mapper;

		public DailyAttendanceSummaryService()
		{
			sp_cud = "usp_CreateUpdateDeleteRestore_DailyAttendanceSummaries";
			sp_r = "usp_GetAll_DailyAttendanceSummaries";
			_mapper = new Mapper();
		}

		public int Create(DailyAttendanceSummary model)
		{
			return Create(model, sp_cud, CreateUpdate(model, "INSERT"));
		}

		public void Update(DailyAttendanceSummary model)
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

		public List<DailyAttendanceSummary> GetAll()
		{
			var mapEntity = new Func<IDataReader, DailyAttendanceSummary>(reader => _mapper.MapEntity<DailyAttendanceSummary>(reader));
			return GetAll(sp_r, mapEntity, GetParams());
		}

		public DailyAttendanceSummary GetById(int id)
		{
			var mapEntity = new Func<IDataReader, DailyAttendanceSummary>(reader => _mapper.MapEntity<DailyAttendanceSummary>(reader));
			return GetById(id, sp_r, mapEntity, GetParams(id));
		}

		private List<KeyValuePair<string, object>> CreateUpdate(DailyAttendanceSummary model, string type)
		{
			var id = type == "INSERT" ? 0 : model.DailyAttendanceSummaryId;

			return new List<KeyValuePair<string, object>>
			{
				new("@Type", type),
				new("@DailyAttendanceSummaryId", id),
				new("@SiteId", model.SiteId),
				new("@TotalWorkers", model.TotalWorkers),
				new("@LiveWorkers", model.LiveWorkers),
				new("@AbsentWorkers", model.AbsentWorkers),
				new("@WeekOffWorkers", model.WeekOffWorkers),
				new("@LeaveWorkers", model.LeaveWorkers),
				new("@PresentWorkers", model.PresentWorkers),
				new("@AttendanceDate", model.AttendanceDate)
			};
		}

		private List<KeyValuePair<string, object>> DeleteRestore(int id, string type)
		{
			return new List<KeyValuePair<string, object>>
			{
				new("@Type", type),
				new("@DailyAttendanceSummaryId", id)
			};
		}

		private SqlParameter[] GetParams(int? id = null, int? siteId = null, DateTime? attendanceDate = null)
		{
			return new SqlParameter[]
			{
				new SqlParameter("@DailyAttendanceSummaryId", id ?? (object)DBNull.Value),
				new SqlParameter("@SiteId", siteId ?? (object)DBNull.Value),
				new SqlParameter("@AttendanceDate", attendanceDate ?? (object)DBNull.Value)
			};
		}
	}


}
