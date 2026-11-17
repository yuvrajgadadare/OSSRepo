using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Dashboard
{
	public class DailyAttendanceSummary
	{
		[Key]
		public int DailyAttendanceSummaryId { get; set; }

		[Required]
		public int SiteId { get; set; }

		public string? SiteName { get; set; }

		[Required]
		public int TotalWorkers { get; set; }

		[Required]
		public int LiveWorkers { get; set; }

		[Required]
		public int AbsentWorkers { get; set; }

		[Required]
		public int WeekOffWorkers { get; set; }

		[Required]
		public int LeaveWorkers { get; set; }

		[Required]
		public int PresentWorkers { get; set; }

		[Required]
		[DataType(DataType.Date)]
		public DateTime AttendanceDate { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime CreatedAt { get; set; } = DateTime.Now;

		[DataType(DataType.DateTime)]
		public DateTime UpdatedAt { get; set; } = DateTime.Now;

	}
}
