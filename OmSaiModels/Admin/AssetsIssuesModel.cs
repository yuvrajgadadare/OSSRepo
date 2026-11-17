using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Admin
{


	public class AssetIssueViewModel
	{
		public int WorkerId { get; set; }
		public List<AssetCheckboxItem> AvailableAssets { get; set; } = new();

		[Required(ErrorMessage = "Issue date and time are required.")]
		public DateTime IssueAt { get; set; }


		public List<int> SelectedAssetIds { get; set; } = new();
	}

	public class AssetCheckboxItem
	{
		public int AssetId { get; set; }
		public int AssetIssueId { get; set; } // Add this property

		public string AssetName { get; set; } = string.Empty;
		public bool IsChecked { get; set; }
		public bool IsDisabled { get; set; }
		
	}

	public class AssetsIssuesModel
    {
        [Key]
        public int? AssetIssueId { get; set; }

        [Required]
        public int WorkerId { get; set; }

        public string? WorkerName { get; set; }

        [Required]
        public int AssetId { get; set; }

        public string? AssetName { get; set; }

       
        public string? IssueBy { get; set; }

        public string? IssueByName {  get; set; }


        public DateTime? IssueAt { get; set; }

      
        public int? ReturnTo { get; set; }

         
        public DateTime? ReturnAt { get; set; }


        public bool IsReturnable { get; set; } 
        
        public DateTime? CreatedAt { get; set; }

       
        public int? CreatedBy { get; set; }

         
        public DateTime? UpdatedAt { get; set; }

        
        public int? UpdatedBy { get; set; }

        public string? Remark { get; set; }


        public bool Status { get; set; } = true;
    }
}
