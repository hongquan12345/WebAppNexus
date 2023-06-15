using NexusApp.Areas.Financial.Models;
using System.ComponentModel.DataAnnotations;

namespace NexusApp.Areas.Customer.Models
{
    public class AccountModel
    {
        public int AccountId { get; set; }
        [Required]
        public string? AccountCode { get; set; }
        [Required]
        public string? Status { get; set; }
        [Required]
        public string? Password { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? CreatedDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? UpdatedDate { get; set; }
        public int CustomerRefId { get; set; }
        public CustomerModel? Customer { get; set; }
        public virtual ICollection<PaymentModel>? Payments { get; set; }
    }
}
