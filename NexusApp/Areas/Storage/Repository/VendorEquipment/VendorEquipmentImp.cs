using Microsoft.EntityFrameworkCore;
using NexusApp.Areas.Storage.Models;
using NexusApp.Data;

namespace NexusApp.Areas.Storage.Repository.VendorEquipment
{
    public class VendorEquipmentImp : IVendorEquipmentRepository
    {
        private readonly ApplicationDbContext context;
        public VendorEquipmentImp(ApplicationDbContext _context)
        {
            context = _context;
        }
        public class VendorEquipmentException : Exception
        {
            public VendorEquipmentException(string message) : base(message) { }
        }
        public async Task AddVendorEquipment(Vendor_Equipment vendorequipment)
        {
            if (vendorequipment != null)
            {
                await context.Vendor_Equipments.AddAsync(vendorequipment);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new VendorEquipmentException("Can not Add VendorEquipment ");
            }
        }

        public async Task DeleteVendorEquipment(int id)
        {
            var vendorequipment = await context.Vendor_Equipments.FindAsync(id);
            if (vendorequipment != null)
            {
                context.Vendor_Equipments.Remove(vendorequipment);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new VendorEquipmentException("Can not Delete VendorEquipment with this ID");
            }
        }

        public async Task<List<Vendor_Equipment>> GetAllVendorEquipment()
        {
            var vendorequipment = await context.Vendor_Equipments.Include(v=>v.Vendor).Include(e=>e.Equipment).ToListAsync();
            if (vendorequipment != null)
            {
                return vendorequipment;
            }
            else
            {
                throw new VendorEquipmentException("Can not get list VendorEquipment");
            }
        }

        public async Task<Vendor_Equipment> GetVendorEquipmentByID(int id)
        {
            var vendorequipment = await context.Vendor_Equipments.FindAsync(id);
            if (vendorequipment != null)
            {
                return vendorequipment;
            }
            else
            {
                throw new VendorEquipmentException("Can not get VendorEquipment by ID");
            }
        }

        public async Task UpdateVendorEquipment(Vendor_Equipment vendorequipment)
        {
            var vendorequip = await context.Vendor_Equipments.FindAsync(vendorequipment.Id);

            if (vendorequip != null)
            {
                vendorequip.Id = vendorequipment.Id;
                vendorequip.VendorRefId = vendorequipment.VendorRefId;
                vendorequip.EquipmentRefId = vendorequipment.EquipmentRefId;
                vendorequip.Quantity = vendorequipment.Quantity;
                context.Vendor_Equipments.Update(vendorequip);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new VendorEquipmentException("Can not Update VendorEquipment ");
            }
        }
    }
}
