using NexusApp.Areas.Employee.Models;
using System.ComponentModel.DataAnnotations;

namespace NexusApp.Areas.RetailShop.Models
{
    public class RetailShopModel
    {
        public int RetailShopId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM}")]
        public DateTime? CreatedDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM}")]
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<EmployeeModel> Employees { get; set; }
    }
}
