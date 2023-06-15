using NexusApp.Areas.Employee.Models;
using System.ComponentModel.DataAnnotations;

namespace NexusApp.Areas.Storage.Models
{
    public class StorageModel
    {
        public int StorageId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Location { get; set; } = string.Empty;
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? CreatedDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? UpdatedDate { get; set; }
        public int EmployeeRefId { get; set; }
        public EmployeeModel? Employee { get; set; }
        public virtual ICollection<EquipmentModel>? Equipments { get; set; }

    }
}

