using NexusApp.Areas.Financial.Models;
using NexusApp.Areas.RetailShop.Models;
using NexusApp.Areas.Storage.Models;
using NexusApp.Areas.Survey.Models;
using System.ComponentModel.DataAnnotations;
namespace NexusApp.Areas.Employee.Models
{
    public class EmployeeModel
    {

        public int EmployeeId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Position { get; set; }
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime StartedDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? CreatedDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? UpdatedDate { get; set; }

        public int RetaishopRefId { get; set; }
        public RetailShopModel RetailShop { get; set; }
        public virtual ICollection<StorageModel> Storages { get; set; }
        public virtual ICollection<SurveyModel> Surveys { get; set; }
        public virtual ICollection<GuaranteeModel> Guarantees { get; set; }
        public virtual ICollection<OrderDetailModel> OrderDetails { get; set; }
    }
}
