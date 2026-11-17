using OmSaiModels.Admin;
using OmSaiModels.Dashboard;
using OmSaiServices.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Reports.Interfaces
{
	public interface IDailyAttendanceSummaryService : IRepository<DailyAttendanceSummary>
	{
	}
}
