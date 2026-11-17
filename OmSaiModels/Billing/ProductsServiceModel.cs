using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Billing
{
	public class ProductsServiceModel
	{
		[Key]
		public int ProductsServiceId { get; set; }

		[Required]
		[StringLength(10)]
		public string ItemType { get; set; }

		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		[StringLength(500)]
		public string Description { get; set; }

		[Required]
		[Range(0, double.MaxValue)]
		public decimal Price { get; set; }

		[StringLength(50)]
		public string Unit { get; set; } = "Unit"; // Default to "Unit" or make it nullable
		[StringLength(50)]
		public string HSNSACCode { get; set; }

		[StringLength(50)]
		public string TaxCategory { get; set; }

		public decimal? TaxPercentage { get; set; }
		public decimal? CGST { get; set; }
		public decimal? SGST { get; set; }
		public decimal? IGST { get; set; }
		public decimal? DiscountPercentage { get; set; }
		public decimal? FinalPrice { get; set; }
		public bool TaxInclusive { get; set; }
		public bool IsActive { get; set; } = true;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime? UpdatedAt { get; set; }
		[StringLength(50)]
		public string? UpdatedIP { get; set; }
		public bool IsDeleted { get; set; } = false;
	}
}
