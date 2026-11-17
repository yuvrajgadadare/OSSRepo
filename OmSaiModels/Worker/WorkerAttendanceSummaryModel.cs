using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Worker
{
    public  class WorkerAttendanceSummaryModel
    {
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int LeaveCount { get; set; }
        public int MissCount { get; set; }

    }

	public class WorkerListForSupervisor
	{
		public int WorkerId { get; set; }
		public string WorkerName { get; set; }
		public string WorkmanId { get; set; }
		public string? ProfileImage { get; set; }

		public int SiteId { get; set; }
		public string SiteName { get; set; }

		public string? ShiftName { get; set; }
		public TimeSpan? StartTime { get; set; }
		public TimeSpan? EndTime { get; set; }

		public int LeaveRequestId { get; set; }

		public int? LeaveTypeId { get; set; }
		public DateOnly? StartDate { get; set; }
		public DateOnly? EndDate { get; set; }
		public string? Reason { get; set; }
		public string? LeaveStatus { get; set; }

		public DateTime? InTime { get; set; }
		public DateTime? OutTime { get; set; }

		public string? InSelfiPath { get; set; }
		public string? OutSelfiPath { get; set; }

		public string Status { get; set; }
	}


}
