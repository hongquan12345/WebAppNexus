using NexusApp.Areas.Customer.Models;
using NexusApp.Areas.Employee.Models;
using NexusApp.Areas.Survey.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusApp.Areas.Financial.Models
{
    public class PaymentModel
    {

        public int PaymentId { get; set; }
        [Required]

        public decimal PaymentAmount { get; set; }
         public bool SendMail { get; set; }
        [Required]
        public string PaymentMode { get; set; } = string.Empty;
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? CreatedDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? UpdatedDate { get; set; }
         public int OrderDetailRefId { get; set; }
        public OrderDetailModel? OrderDetails { get; set; }
        public int GuaranteeRefId { get; set; }
        public GuaranteeModel Guarantees { get; set; }     
        public int AccountRefId { get; set; }
        public AccountModel Accounts { get; set; }
    }
}
