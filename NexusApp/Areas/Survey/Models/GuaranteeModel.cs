using NexusApp.Areas.Customer.Models;
using NexusApp.Areas.Employee.Models;
using NexusApp.Areas.Financial.Models;
using System.ComponentModel.DataAnnotations;

namespace NexusApp.Areas.Survey.Models
{
    public class GuaranteeModel
    {
        public int GuaranteeId { get; set; }
        public bool IsDeposit { get; set; }
        public Decimal Amount { get; set; }
        public bool SendMail { get; set; }
        public string? Note { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime DepositedDate { get; set; } 
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime CreatedDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? UpdatedDate { get; set; }
        public int SurveyRefId { get; set; }
        public SurveyModel? surveyModel { get; set; }
        public virtual ICollection<PaymentModel>? Payments { get; set; }
    }
}
