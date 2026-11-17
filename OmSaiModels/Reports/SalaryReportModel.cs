using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Reports
{
	public class SalaryReportModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public DateTime CreatedDate { get; set; }
	}
	public class SalarySheet
	{
		public int SrNo { get; set; }
		public string NameOfStaff { get; set; }
		public string Designation { get; set; }
		public string DateOfJoining { get; set; }
		public int TotalDays { get; set; }
		public decimal GrossWages { get; set; }
		public decimal DAForSalary { get; set; }
		public decimal PF { get; set; }
		public decimal ESIC { get; set; }
		public decimal ProfTax { get; set; }
		public decimal Advance { get; set; }
		public decimal TotalDeductions { get; set; }
		public string DeductionRemark { get; set; }
		
		public decimal NetSalary { get; set; }
		public decimal PaidAmount { get; set; }
		
		public string PaymentMode { get; set; }
		public string Remark { get; set; }
		public string AccountNumber { get; set; }
		public string BankName { get; set; }
		public string IFSCCode { get; set; }
		public string Branch { get; set; }
	}


	public class WorkerMonthlySalaryReportModel
	{
		public int Year { get; set; }
		public int Month { get; set; }
		public string StaffName { get; set; }
		public string DepartmentName { get; set; }
		public DateTime DateofJoining { get; set; }

		public decimal ShiftHrs { get; set; }
		public decimal MonthlySalary { get; set; }
		public decimal TotalMonthlyHrs { get; set; }
		public decimal TotalWorkingHrs { get; set; }
		public decimal GrossAmount { get; set; }
		public decimal DAAmount { get; set; }
		public decimal EPFAmount { get; set; }
		public decimal ESICAmount { get; set; }
		public decimal PTAmount { get; set; }
		public decimal AdvanceAmount { get; set; }
		public decimal DeductionAmount { get; set; }
		public string DeductionRemark { get; set; }
		public decimal TotalDeduction{ get; set; }

		public decimal NetSalary { get; set; }
		public decimal PaidAmount { get; set; }
		public string PaymentMode { get; set; }
		public string AccountNumber { get; set; }
		public string IfscCode { get; set; }
		public string BankName { get; set; }
		public string BranchName { get; set; }
	}

}
