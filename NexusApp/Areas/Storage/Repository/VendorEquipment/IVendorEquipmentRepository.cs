using NexusApp.Areas.Storage.Models;

namespace NexusApp.Areas.Storage.Repository.VendorEquipment
{
    public interface IVendorEquipmentRepository
    {
        Task<List<Vendor_Equipment>> GetAllVendorEquipment();
        Task<Vendor_Equipment> GetVendorEquipmentByID(int id);
        Task AddVendorEquipment(Vendor_Equipment vendorequipment);
        Task UpdateVendorEquipment(Vendor_Equipment vendorequipment);
        Task DeleteVendorEquipment(int id);
    }
}
