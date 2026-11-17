using System;
using System.ComponentModel.DataAnnotations;

namespace OmSaiModels.Invoice
{
	public class InvoiceItemModel
	{
		public int InvoiceItemId { get; set; }
		public int InvoiceId { get; set; }

		[Required]
		public int ProductsServiceId { get; set; }

		public string? Description { get; set; }
		public string? HsnSac { get; set; }

		public int Qty { get; set; } = 1;

		[Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative")]
		public decimal CurrentPrice { get; set; }

		public string? Unit { get; set; }

		public int PresentDays { get; set; }

		// Tax percentages
		public decimal? CGSTPercentage { get; set; }
		public decimal? SGSTPercentage { get; set; }
		public decimal? IGSTPercentage { get; set; }

		// Add these calculated fields
		public decimal CGSTAmount { get; set; }
		public decimal SGSTAmount { get; set; }
		public decimal IGSTAmount { get; set; }

		public decimal Amount { get; set; }


		public bool IsActive { get; set; } = true;

		public string? UpdatedIP { get; set; }

		// Add these for tracking
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime? UpdatedAt { get; set; }
	}

}
