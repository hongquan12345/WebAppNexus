using NexusApp.Areas.Financial.Models;
using NexusApp.Areas.ServiceConnection.Models;
using NexusApp.Areas.Survey.Models;
using System.ComponentModel.DataAnnotations;

namespace NexusApp.Areas.Customer.Models
{
    public class CustomerModel
    {

        public int CustomerId { get; set; }
        [Required]
        public string Name { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime? BirthDay { get; set; }
        public string? Street { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; }
        [Required]
        public string Phone { get; set; } = string.Empty;
        public bool RegistrationStatus { get; set; } // True: New False: Old

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? CreatedDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? UpdatedDate { get; set; }

        public int ServiceRefId { get; set; }
        public ServiceModel? Services { get; set; }
        public virtual ICollection<SurveyModel>? Surveys { get; set; }
        public virtual ICollection<GuaranteeModel>? Guarantees{ get; set; }
        public virtual ICollection<OrderDetailModel>? OrderDetails{ get; set; }
        public virtual AccountModel? Accounts { get; set; }

    }
}
