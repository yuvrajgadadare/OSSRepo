using OmSaiModels.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Reports.Interfaces
{
	public interface IWorkerSalaryReportService
	{
		List<WorkerMonthlySalaryReportModel> SiteWiseExcelReport(int Year, int Month, int? SiteId);
	}
}
