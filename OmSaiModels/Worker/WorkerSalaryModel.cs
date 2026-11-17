using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Worker
{
   
    public class SalaryProjectSitesModel
    {
        [Key]
        public int? WorkerSalaryId { get; set; }
        public int WorkerId { get; set; }
        public string? WorkerName { get; set; }
        public string? SalaryType { get; set; }
		public int SiteId { get; set; }
		public int ShiftHrs { get; set; } = 8;
		
		public decimal? HourlyWage { get; set; } = 0;

		public decimal? DAAmount { get; set; } = 0m;  // ➕ Dearness Allowance (Added to Gross)
		public decimal? EPFAmount { get; set; } = 0m; // ➖ Employee Provident Fund (Deducted)
		public decimal? ESICAmount { get; set; } = 0m; // ➖ Employee State Insurance (Deducted)
		public decimal? PTAmount { get; set; } = 0m;  // ➖ Professional Tax (Deducted)

		public decimal? MonthlySalary { get; set; }

	}


	public class WorkerMonthlySalaryEdit
	{
		[Key]
		public int WorkerMonthlySalaryId { get; set; }

		[Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal AdvanceAmount { get; set; } = 0;

		[Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal OtherAllowances { get; set; } = 0;

		[MaxLength(255)]
		public string? OtherAllowancesRemark { get; set; }

		[Column(TypeName = "decimal(10,2)")]
		public decimal? DeductionAmount { get; set; } = 0;

		[MaxLength(255)]
		public string? DeductionRemark { get; set; }

		[Column(TypeName = "decimal(10,2)")]
		public decimal? TotalDeduction { get; set; } = 0;

		[Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal NetSalary { get; set; }

		[Column(TypeName = "decimal(10,2)")]
		public decimal? PaidAmount { get; set; } = 0;

		[MaxLength(255)]
		public string? PaymentMode { get; set; }



	}

	public class WorkerMonthlySalary
	{
		[Key]
		public int WorkerMonthlySalaryId { get; set; }

		[Required]
		public int WorkerId { get; set; }
		public string WorkerName { get; set; }
		
		[Required]
		public int SiteId { get; set; }
		
		public string SiteName { get; set; }

		public int? ShiftId { get; set; }
		public int ShiftHrs { get; set; } = 8;
		
		public string? ShiftName { get; set; }

		[Required]
		public int Month { get; set; } // 1 to 12

		[Required]
		public int Year { get; set; }

		[Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal HourlyWage { get; set; }

		[Required]
		public int TotalMonthlyHrs { get; set; }

		[Required]
		public int TotalWorkingHrs { get; set; }

		[Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal MonthlySalary { get; set; }

		[Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal GrossAmount { get; set; } = 0;

		[Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal DAAmount { get; set; } = 0;

		[Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal EPFAmount { get; set; } = 0;

		[Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal ESICAmount { get; set; } = 0;

		[Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal PTAmount { get; set; } = 0;

		[Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal AdvanceAmount { get; set; } = 0;


		[Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal OtherAllowances { get; set; } = 0;

		[MaxLength(255)]
		public string? OtherAllowancesRemark { get; set; }

		[Column(TypeName = "decimal(10,2)")]
		public decimal? DeductionAmount { get; set; } = 0;

		[MaxLength(255)]
		public string? DeductionRemark { get; set; }

		[Column(TypeName = "decimal(10,2)")]
		public decimal? TotalDeduction { get; set; } = 0;

		[Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal NetSalary { get; set; }

		[Column(TypeName = "decimal(10,2)")]
		public decimal? PaidAmount { get; set; } = 0;

		[MaxLength(255)]
		public string? PaymentMode { get; set; }

		[MaxLength(50)]
		public string? AccountNumber { get; set; }

		[MaxLength(20)]
		public string? IfscCode { get; set; }

		[MaxLength(50)]
		public string? BankName { get; set; }

		[MaxLength(100)]
		public string? BranchName { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

	
	}




}
