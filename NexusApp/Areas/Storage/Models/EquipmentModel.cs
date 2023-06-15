using NexusApp.Areas.Financial.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusApp.Areas.Storage.Models
{
    public class EquipmentModel
    {
        public int EquipmentId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int Serial { get; set; }
        public bool IsSupportLine { get; set; } // checkbox
        public bool IsSupportInternet { get; set; } // checkbox
        [Required]
        public decimal Price { get; set; }
        public string? Type { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? CreatedDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:MM:SS}")]
        public DateTime? UpdatedDate { get; set; }
        [ForeignKey(nameof(Storage))]
        public int StorageRefId { get; set; }
        public StorageModel? Storage { get; set; }
        public virtual ICollection<Vendor_Equipment>? Vendor_Equipment { get; set; }
        public virtual ICollection<OrderDetailModel>? OrderDetails { get; set; }

    }
}
