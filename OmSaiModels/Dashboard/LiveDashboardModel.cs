using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Dashboard
{
	public class LiveDashboardModel
	{

	}

	public class AttendanceSummaryModel
	{
		public int TotalWorkers { get; set; }
		public int PresentWorkers { get; set; }
		public int AbsentWorkers { get; set; }
		public int LiveWorkers { get; set; }

	}

	public class AttendanceSummaryLast7DaysModel
	{
		public DateOnly Date { get; set; }
		public string DayName { get; set; }
		public int TotalActiveWorkers { get; set; }
		public int PresentWorkers { get; set; }
		public int AbsentWorkers { get; set; }
		public int LiveWorkers { get; set; }
		public float AttendancePercentage { get; set; }

	}

	
	public class AttendanceSummaryBySitesModel
	{
		public int SiteId { get; set; }
		public string SiteName { get; set; }
		public int TotalWorkers { get; set; }
		public int PresentWorkers { get; set; }
		public int AbsentWorkers { get; set; }
		public int LiveWorkers { get; set; }
		public int WeekOffWorkers { get; set; }
		public int LeaveWorkers { get; set; }

	}


	public class DailyAttendanceSummaryBySite
	{
		public int SiteId { get; set; }
		public string SiteName { get; set; } = string.Empty;

		public DateTime AttendanceDate { get; set; }

		public int TotalWorkers { get; set; }
		public int PresentWorkers { get; set; }
		public int LiveWorkers { get; set; }
		public int AbsentWorkers { get; set; }
		public int WeekOffWorkers { get; set; }
		public int LeaveWorkers { get; set; }

		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}


}
