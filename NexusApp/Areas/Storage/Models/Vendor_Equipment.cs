using System.ComponentModel.DataAnnotations;

namespace NexusApp.Areas.Storage.Models
{
    public class Vendor_Equipment
    {
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int VendorRefId { get; set; }
        public VendorModel? Vendor { get; set; }
        [Required]
        public int EquipmentRefId { get; set; }
        public EquipmentModel? Equipment { get; set; }
    }
}
