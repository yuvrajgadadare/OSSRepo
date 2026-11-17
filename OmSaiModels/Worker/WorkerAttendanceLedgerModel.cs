using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmSaiModels.Admin;

namespace OmSaiModels.Worker
{


	public class WorkerAttendanceLedgerModel
	{
		[Key]
		public int WorkerAttendanceLedgerId { get; set; }

		[Required]
		public int WorkerId { get; set; }
		public string? WorkerName { get; set; }
		public string? WorkmanId { get; set; }
		
		[Required]
		public int SiteId { get; set; }
		public string? SiteName { get; set; }

		public int? SiteShiftId { get; set; }
		public string? ShiftName { get; set; }
		public string? StartTime { get; set; }
		public string? EndTime { get; set; }


		[Required]
		public int Year { get; set; }

		[Required]
		public int Month { get; set; }

		[StringLength(2)]
		public int Day1 { get; set; }

		[StringLength(2)]
		public int Day2 { get; set; }

		[StringLength(2)]
		public int Day3 { get; set; }

		[StringLength(2)]
		public int Day4 { get; set; }

		[StringLength(2)]
		public int Day5 { get; set; }

		[StringLength(2)]
		public int Day6 { get; set; }

		[StringLength(2)]
		public int Day7 { get; set; }

		[StringLength(2)]
		public int Day8 { get; set; }

		[StringLength(2)]
		public int Day9 { get; set; }

		[StringLength(2)]
		public int Day10 { get; set; }

		[StringLength(2)]
		public int Day11 { get; set; }

		[StringLength(2)]
		public int Day12 { get; set; }

		[StringLength(2)]
		public int Day13 { get; set; }

		[StringLength(2)]
		public int Day14 { get; set; }

		[StringLength(2)]
		public int Day15 { get; set; }
		
		[StringLength(2)]
		public int Day16 { get; set; }
		
		[StringLength(2)]
		public int Day17 { get; set; }
		
		[StringLength(2)]
		public int Day18 { get; set; }
		
		[StringLength(2)]
		public int Day19 { get; set; }
		
		[StringLength(2)]
		public int Day20 { get; set; }
		
		[StringLength(2)]
		public int Day21 { get; set; }
		
		[StringLength(2)]
		public int Day22 { get; set; }
		
		[StringLength(2)]
		public int Day23 { get; set; }
		
		[StringLength(2)]
		public int Day24 { get; set; }
		
		[StringLength(2)]
		public int Day25 { get; set; }
		
		[StringLength(2)]
		public int Day26 { get; set; }
		
		[StringLength(2)]
		public int Day27 { get; set; }
		
		[StringLength(2)]
		public int Day28 { get; set; }
		
		[StringLength(2)]
		public int Day29 { get; set; }
		
		[StringLength(2)]
		public int Day30 { get; set; }
		
		[StringLength(2)]
		public int Day31 { get; set; }
		
		public int? TotalHrs { get; set; }
		public int? TotalDays { get; set; }
		public int? DaysPresent { get; set; }
		public int? DaysAbsent { get; set; }
		public int? DaysHalfDay { get; set; }
		public int? DaysLeave { get; set; }
		public int? DaysMiss { get; set; }

		//public WorkerModel? Worker { get; set; } // Assuming there is a Worker model

		//public SiteModel? Site { get; set; } // Assuming there is a Site model
	}


	public class LedgerViewModel
	{
		public int? SiteId { get; set; }

		public int? SiteShiftId { get; set; }

		public int? Year { get; set; }

		public int? Month { get; set; }
	}


	public class GreaterThanZeroAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			// Handle null or empty value (required validation)
			if (value == null || (value is string str && string.IsNullOrEmpty(str)))
			{
				return new ValidationResult($"{validationContext.DisplayName} is required.");
			}

			// Check if value is a valid integer and greater than zero
			if (value is not int intValue || intValue <= 0)
			{
				return new ValidationResult($"{validationContext.DisplayName} must be greater than zero.");
			}

			return ValidationResult.Success;
		}
	}




}
