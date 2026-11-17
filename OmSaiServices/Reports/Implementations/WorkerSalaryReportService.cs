using DocumentFormat.OpenXml.Bibliography;
using LmsServices.Common;
using Microsoft.Data.SqlClient;
using OmSaiModels.Admin;
using OmSaiModels.Reports;
using OmSaiServices.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Reports.Implementations
{
	public class WorkerSalaryReportService : IWorkerSalaryReportService
	{
		private readonly string usp_excelReport;
		private readonly Mapper _mapper;

		public WorkerSalaryReportService()
		{
			usp_excelReport = "usp_reports_WorkerMonthlySalaries";
			_mapper = new Mapper();
		}

		public List<WorkerMonthlySalaryReportModel> SiteWiseExcelReport(int Year, int Month, int? SiteId = null)
		{
			// Define the mapping function
			var mapEntity = new Func<IDataReader, WorkerMonthlySalaryReportModel>(reader => _mapper.MapEntity<WorkerMonthlySalaryReportModel>(reader));

			// Define parameters for stored procedure
			var parameters = new SqlParameter[]
			{
				new SqlParameter("@Year", Year),
				new SqlParameter("@Month", Month),
				new SqlParameter("@SiteId", SiteId)
			};

			return QueryService.Query(usp_excelReport, mapEntity, parameters);
		}

	}
}
