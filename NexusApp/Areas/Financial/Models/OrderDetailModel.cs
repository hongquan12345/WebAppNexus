using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NexusApp.Areas.Customer.Models;
using NexusApp.Areas.Employee.Models;
using NexusApp.Areas.Storage.Models;

namespace NexusApp.Areas.Financial.Models
{
    public class OrderDetailModel
    {
        public int OrderDetailId { get; set; }
        [Required]
        public string OrderSeri { get; set; }
        [Required]
        public bool IsEquipment { get; set; } // Yes - No
        [Required]
        public int? Quantity { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? CreatedDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? UpdatedDate { get; set; }
        public int? EquipmentRefId { get; set; } //Defaule = 0 "Non-Equipment"
        public EquipmentModel? Equipments { get; set; }
         public int ConnectionRefId { get; set; }
        public ConnectionModel? Connections { get; set; }    
        public int EmployeeRefId { get; set; }
        public EmployeeModel? Employees { get; set; }    
        public int CustomerRefId { get; set; }
        public CustomerModel? Customers { get; set; }
        public virtual ICollection<PaymentModel>? Payments { get; set; }
    }
}
