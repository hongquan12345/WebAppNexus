using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using NexusApp.Areas.Storage.Models;
using NexusApp.Data;
using static NexusApp.Areas.Storage.Repository.Storage.StorageImp;

namespace NexusApp.Areas.Storage.Repository.Vendor
{
    public class VendorImp : IVendorRepository
    {
        private readonly ApplicationDbContext context;
        public VendorImp(ApplicationDbContext _context)
        {
            context = _context;
        }
        public class VendorException : Exception
        {
            public VendorException(string message) : base(message) { }
        }
        public async Task AddVendor(VendorModel vendor)
        {
            if (vendor != null)
            {
                vendor.CreatedDate = DateTime.Now;
                await context.vendorModels.AddAsync(vendor);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new VendorException("Can not Add Vendor ");
            }
        }

        public async Task DeleteVendor(int id)
        {
            var vendor = await context.vendorModels.FindAsync(id);
            if (vendor != null)
            {
                context.vendorModels.Remove(vendor);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new VendorException("Can not Delete Vendor with this ID");
            }
        }

        public async Task<List<VendorModel>> GetAllVendor()
        {
            var vendor = await context.vendorModels.ToListAsync();
            if (vendor != null)
            {
                return vendor;
            }
            else
            {
                throw new VendorException("Can not get list Vendor ");
            }
        }

        public async Task<VendorModel> GetVendorByID(int id)
        {
            var vendor = await context.vendorModels.FindAsync(id);
            if (vendor != null)
            {
                return vendor;
            }
            else
            {
                throw new VendorException("Can not get Vendor by ID");
            }
        }

        public async Task UpdateVendor(VendorModel vendor)
        {
            var ven = await context.vendorModels.FindAsync(vendor.VendorId);

            if (ven != null)
            {
                ven.Name = vendor.Name;
                ven.Address = vendor.Address;
                ven.Phone = vendor.Phone;
                ven.Email = vendor.Email;
                ven.UpdatedDate = DateTime.Now;
                context.vendorModels.Update(ven);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new VendorException("Can not Update Vendor ");
            }
        }
    }
}
