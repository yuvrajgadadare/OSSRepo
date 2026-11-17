using DocumentFormat.OpenXml.Spreadsheet;
using LmsServices.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using OmSaiModels.Billing;
using OmSaiServices.Billing.Interfaces;
using OmSaiServices.Common;
using System;
using System.Collections.Generic;
using System.Data;

namespace OmSaiServices.Billing.Implementations
{
	public class InvoiceService : Repository<InvoiceModel>, IInvoiceServices
	{
		private readonly string usp_cud;
		private readonly string usp_r;
		private readonly Mapper _mapper;

		public InvoiceService()
		{
			usp_cud = "usp_CreateUpdateDeleteRestore_Invoices";
			usp_r = "usp_GetAll_Invoices";
			_mapper = new Mapper();
		}

		public int Create(InvoiceModel model)
		{
			return Create(model, usp_cud, CreateUpdate(model, "INSERT"));
		}

		public void Update(InvoiceModel model)
		{
			Update(model, usp_cud, CreateUpdate(model, "UPDATE"));
		}

		public void Delete(int id)
		{
			Delete(usp_cud, DeleteRestore(id, "DELETE"));
		}

		public void Restore(int id)
		{
			Restore(usp_cud, DeleteRestore(id, "RESTORE"));
		}

		public void PaymentStatus(int id, string paymentStatus)
		{
			//var UpdatedIP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

			string outputParameterName = "@LastInsertedId";
			// Add a @type parameter manually

			var parameters = new List<KeyValuePair<string, object>>
			{
				new("@InvoiceId", id),
				new("@PaymentStatus", paymentStatus),
				new("@UpdatedIP", "Unknown"),
				new("@Type", "PAYSTATUS")				
			};
			QueryService.NonQuery(usp_cud, parameters, outputParameterName);
		}


		private List<KeyValuePair<string, object>> DeleteRestore(int id, string type)
		{
			return new List<KeyValuePair<string, object>>
			{
				new("@InvoiceId", id)
			};
		}

		public List<InvoiceModel> GetAll(int? invoiceId = null, string invoiceNumber = null, string? workOrderNumber = null, int? billingFromId = null, int? billingCustomerId = null, int? serviceMonth = null, int? serviceYear = null, string paymentStatus = null)
		{
			var mapEntity = new Func<IDataReader, InvoiceModel>(reader => _mapper.MapEntity<InvoiceModel>(reader));

			return GetAll(usp_r, mapEntity, GetParams(invoiceId, invoiceNumber, workOrderNumber, billingFromId, billingCustomerId, serviceMonth, serviceYear, paymentStatus));
		}

		public InvoiceModel GetById(int id)
		{
			var mapEntity = new Func<IDataReader, InvoiceModel>(reader => _mapper.MapEntity<InvoiceModel>(reader));

			return GetById(id, usp_r, mapEntity, GetParams(id));
		}

		private List<KeyValuePair<string, object>> CreateUpdate(InvoiceModel model, string type)
		{
			var invoiceId = type == "INSERT" ? 0 : model.InvoiceId;
			
			return new List<KeyValuePair<string, object>>
			{
				new("@InvoiceId", invoiceId),
				// Add SubTotal parameter
				new("@SubTotal", model.SubTotal),
				new("@TotalQuantity", model.TotalQuantity),
				new("@InvoiceNumber", model.InvoiceNumber),
				new("@InvoiceDate", model.InvoiceDate),
				new("@WorkOrderNumber", model.WorkOrderNumber),
				new("@WorkOrderDate", model.WorkOrderDate),
				new("@BillingFromId", model.BillingFromId),
				new("@BillingCustomerId", model.BillingCustomerId),
				new("@ServiceMonth", model.ServiceMonth),
				new("@ServiceYear", model.ServiceYear),
				new("@TotalItemAmount", model.TotalItemAmount),
				new("@DiscountAmount", model.DiscountAmount),
				new("@CGstPercentage", model.CGstPercentage),
				new("@SGstPercentage", model.SGstPercentage),
				new("@IGstPercentage", model.IGstPercentage),
				new("@CGstAmount", model.CGstAmount),
				new("@SGstAmount", model.SGstAmount),
				new("@IGstAmount", model.IGstAmount),
				new("@TotalInvoiceAmount", model.TotalInvoiceAmount),
				new("@GrandTotal", model.GrandTotal),
				new("@PaymentStatus", model.PaymentStatus),
				new("@UpdatedIP", model.UpdatedIP ?? (object)DBNull.Value)
			};
		}

		private SqlParameter[] GetParams(int? invoiceId = null, string invoiceNumber = null, string? workOrderNumber = null, int? billingFromId = null, int? billingCustomerId = null, int? serviceMonth = null, int? serviceYear = null, string paymentStatus = null)
		{
			return new SqlParameter[]
			{
				new SqlParameter("@InvoiceId", invoiceId ?? (object)DBNull.Value),
				new SqlParameter("@InvoiceNumber", invoiceNumber ?? (object)DBNull.Value),
				new SqlParameter("@WorkOrderNumber", workOrderNumber ?? (object)DBNull.Value),
				new SqlParameter("@BillingFromId", billingFromId ?? (object)DBNull.Value),
				new SqlParameter("@BillingCustomerId", billingCustomerId ?? (object)DBNull.Value),
				new SqlParameter("@ServiceMonth", serviceMonth ?? (object)DBNull.Value),
				new SqlParameter("@ServiceYear", serviceYear ?? (object)DBNull.Value),
				new SqlParameter("@PaymentStatus", paymentStatus ?? (object)DBNull.Value)
			};
		}
	}
}
