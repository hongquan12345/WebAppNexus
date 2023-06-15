using NexusApp.Areas.Customer.Models;
using NexusApp.Areas.Employee.Models;
using System.ComponentModel.DataAnnotations;

namespace NexusApp.Areas.Survey.Models
{
    public class SurveyModel
    {
        public int SurveyId { get; set; }
        [Required]
        public string ServeyName { get; set; }
        [Required]
        public bool IsEquipment { get; set; } // checkbox
        [Required]
        public bool IsSupportInternet { get; set; } // checkbox
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime SurveyDate { get; set; }
        [Required]
        public string Status { get; set; } = string.Empty; //Pending - completed

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime CreatedDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime UpdatedDate { get; set; }
        public string? Descriptiontest { get; set; }
        public int EmployeeRefId { get; set; }
        public EmployeeModel? Employee { get; set; }
        public int CustomerRefId { get; set; }
        public CustomerModel? Customer { get; set; }
        public virtual InstallModel? Installs { get; set; }
        public virtual GuaranteeModel? Guarantees { get; set; }

    }
}
