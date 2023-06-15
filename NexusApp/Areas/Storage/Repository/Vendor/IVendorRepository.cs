using NexusApp.Areas.Storage.Models;

namespace NexusApp.Areas.Storage.Repository.Vendor
{
    public interface IVendorRepository
    {
        Task<List<VendorModel>> GetAllVendor();
        Task<VendorModel> GetVendorByID(int id);
        Task AddVendor(VendorModel vendor);
        Task UpdateVendor(VendorModel vendor);
        Task DeleteVendor(int id);
    }
}
