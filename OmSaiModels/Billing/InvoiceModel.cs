using OmSaiModels.Invoice;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace OmSaiModels.Billing
{

	public class InvoiceModel
	{
		public int InvoiceId { get; set; }
		[Required]
		public string InvoiceNumber { get; set; }
		public DateTime InvoiceDate { get; set; }

		public string WorkOrderNumber { get; set; }
		public DateTime WorkOrderDate { get; set; }


		public int BillingFromId { get; set; }
		public int BillingCustomerId { get; set; }
		public int ServiceMonth { get; set; }
		public int ServiceYear { get; set; }
		public decimal TotalItemAmount { get; set; }
		public decimal? DiscountAmount { get; set; }
		public decimal CGstPercentage { get; set; }
		public decimal SGstPercentage { get; set; }
		public decimal IGstPercentage { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal CGstAmount { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal SGstAmount { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal IGstAmount { get; set; }
		public decimal TotalInvoiceAmount { get; set; }
		public decimal GrandTotal { get; set; }
		public string? PaymentStatus { get; set; } = "Pending"; // Default value, nullable
		public string? UpdatedIP { get; set; } // Nullable
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }


		public string? FromName { get; set; }

		public string? FromEmail { get; set; }

		public string? FromPhone1 { get; set; }

		public string? FromPhone2 { get; set; }

		public string? FromGSTIN { get; set; }

		public string? FromAddress { get; set; }
		public string? FromStateName { get; set; }
		public int? FromStateCode { get; set; }

		public string? FromBankAccountOwnerName { get; set; }

		public string? FromBankName { get; set; }

		public string? FromBankBranchName { get; set; }

		public string? FromBankAccountNumber { get; set; }

		public string? FromBankIFSCCode { get; set; }

		public string? ToName { get; set; }

		public string? ToEmail { get; set; }

		public string? ToPhone1 { get; set; }

		public string? ToPhone2 { get; set; }

		public string? ToGSTIN { get; set; }

		public string? ToAddress { get; set; }
		public string? ToStateName { get; set; }
		public int? ToStateCode { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal SubTotal { get; set; }


		[Column(TypeName = "decimal(18,2)")]
		public decimal TaxAmount { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal TotalAmount { get; set; }

		public int TotalQuantity { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal Discount { get; set; }

	}


	public class InvoicePdfViewModel
	{
		public InvoiceModel Invoice { get; set; }
		public List<InvoiceItemModel> Items { get; set; }
	}
	public class InvoiceCreateViewModel
	{
		[Required]
		public InvoiceModel Invoice { get; set; } = new InvoiceModel();
		[Required]
		public List<InvoiceItemModel> InvoiceItems { get; set; } = new List<InvoiceItemModel>();
		public List<BillingCustomerModel>? Customers { get; set; } // Nullable, populated by controller
		public List<BillingFromModel>? Companies { get; set; } // Nullable, populated by controller
		public List<ProductsServiceModel>? Products { get; set; } // Nullable, populated by controller
	}

}