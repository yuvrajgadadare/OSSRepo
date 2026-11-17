using System;

namespace OmSaiModels.Billing
{
	public class BillingCustomerModel
	{
		public int BillingCustomerId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone1 { get; set; }
		public string Phone2 { get; set; }
		public string Address { get; set; }
		public string? GSTIN { get; set; }
		public string? StateName { get; set; }
		public int? StateCode { get; set; }
		public string? UpdatedIP { get; set; }
	}
}
