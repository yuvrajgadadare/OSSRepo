using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using LmsServices.Common;
using Microsoft.Data.SqlClient;
using OmSaiModels.Admin;
using OmSaiModels.Dashboard;
using OmSaiModels.Worker;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Admin.Implementations
{
	public class LiveDashboardService
	{
		private readonly string usp_attendanceSummary;
		private readonly string usp_weeklyAttendanceSummary;
		private readonly Mapper _mapper;


		public LiveDashboardService()
		{
			usp_attendanceSummary = "usp_GetWorkerAttendanceSummary";
			usp_weeklyAttendanceSummary = "usp_GetWorkerAttendanceSummaryLast7Days";
			
			_mapper = new Mapper();

		}

		public AttendanceSummaryModel GetWorkerAttendanceSummary( DateTime? inputDate = null)
		{
			var mapEntity = new Func<IDataReader, AttendanceSummaryModel>(reader => _mapper.MapEntity<AttendanceSummaryModel>(reader));

			var parameters = new SqlParameter[]
			{
				new SqlParameter("@InputDate", inputDate ?? (object)DBNull.Value)
			};

			var result = QueryService.Query(usp_attendanceSummary, mapEntity, parameters);
			return result?.FirstOrDefault();
		}


		public List<AttendanceSummaryLast7DaysModel>  GetWorkerAttendanceSummaryLast7Days(int? siteId = null)
		{
			var mapEntity = new Func<IDataReader, AttendanceSummaryLast7DaysModel>(reader => _mapper.MapEntity<AttendanceSummaryLast7DaysModel>(reader));

			var parameters = new SqlParameter[]{
				new SqlParameter("@SiteId", siteId),
			};

			return QueryService.Query(usp_weeklyAttendanceSummary, mapEntity, parameters);
		}


		public List<AttendanceSummaryBySitesModel> GetWorkerAttendanceSummaryBySites()
		{
			var mapEntity = new Func<IDataReader, AttendanceSummaryBySitesModel>(reader => _mapper.MapEntity<AttendanceSummaryBySitesModel>(reader));

			var parameters = new SqlParameter[] { };

			return QueryService.Query("usp_CreateUpdateDailyAttendanceSummary", mapEntity, parameters);
		}

		public List<DailyAttendanceSummaryBySite> GetTodayAndYesterdayAttendanceSummaryBySite(int? siteId = null)
		{
			var mapEntity = new Func<IDataReader, DailyAttendanceSummaryBySite>(reader => _mapper.MapEntity<DailyAttendanceSummaryBySite>(reader));

			var parameters = new SqlParameter[] {
				new SqlParameter("@SiteId", siteId)
			};

			return QueryService.Query("usp_GetTodayAndYesterdayAttendanceSummaryBySite", mapEntity, parameters);
		}




	}
}
