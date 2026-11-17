using System.ComponentModel.DataAnnotations;

namespace OmSaiModels.Billing
{
	public class BillingFromModel
	{
		[Key]
		public int BillingFromId { get; set; }

		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		[Required]
		[EmailAddress]
		[StringLength(255)]
		public string Email { get; set; }

		[Required]
		[Phone]
		[StringLength(20)]
		public string Phone1 { get; set; }

		[Phone]
		[StringLength(20)]
		public string? Phone2 { get; set; }

		[Required]
		[StringLength(20)]
		public string GSTIN { get; set; }

		public string? StateName { get; set; }
		public int? StateCode { get; set; }


		[Required]
		[StringLength(500)]
		public string Address { get; set; }

		[Required]
		[StringLength(255)]
		public string BankAccountOwnerName { get; set; }

		[Required]
		[StringLength(255)]
		public string BankName { get; set; }

		[Required]
		[StringLength(255)]
		public string BankBranchName { get; set; }

		[Required]
		[StringLength(50)]
		public string BankAccountNumber { get; set; }

		[Required]
		[StringLength(20)]
		public string BankIFSCCode { get; set; }

		[Required]
		public bool IsActive { get; set; } = true;

		[Required]
		public DateTime CreatedAt { get; set; } = DateTime.Now;

		public DateTime? UpdatedAt { get; set; }

		[StringLength(50)]
		public string? UpdatedIP { get; set; }

		[Required]
		public bool IsDeleted { get; set; } = false;
	}
}
