using Microsoft.EntityFrameworkCore;
using NexusApp.Areas.Storage.Models;
using NexusApp.Data;
using static NexusApp.Areas.Storage.Repository.VendorEquipment.VendorEquipmentImp;

namespace NexusApp.Areas.Storage.Repository.Equipment
{
    public class EquipmentImp : IEquipmentRepository
    {
        private readonly ApplicationDbContext context;
        public EquipmentImp(ApplicationDbContext _context)
        {
            context = _context;
        }
        public class EquipmentException : Exception
        {
            public EquipmentException(string message) : base(message) { }
        }
        public async Task AddEquipment(EquipmentModel equipment)
        {
            if (equipment != null)
            {
                equipment.CreatedDate = DateTime.Now;
                await context.EquipmentModels.AddAsync(equipment);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new EquipmentException("Can not Add Equipment ");
            }
        }

        public async Task DeleteEquipment(int id)
        {
            var equipment = await context.EquipmentModels.FindAsync(id);
            if (equipment != null)
            {
                context.EquipmentModels.Remove(equipment);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new EquipmentException("Can not Delete Equipment with this ID");
            }
        }

        public async Task<List<EquipmentModel>> GetAllEquipment()
        {
            var equipment = await context.EquipmentModels.Include(s=>s.Storage).ToListAsync();
            if (equipment != null)
            {
                return equipment;
            }
            else
            {
                throw new EquipmentException("Can not get list Equipment");
            }
        }

        public async Task<EquipmentModel> GetEquipmentByID(int id)
        {
            var equipment = await context.EquipmentModels.FindAsync(id);
            if (equipment != null)
            {
                return equipment;
            }
            else
            {
                throw new EquipmentException("Can not get Equipment by ID");
            }
        }

        public async Task UpdateEquipment(EquipmentModel equipment)
        {
            var equip = await context.EquipmentModels.FindAsync(equipment.EquipmentId);

            if (equip != null)
            {
                equip.EquipmentId = equipment.EquipmentId;
                equip.StorageRefId = equipment.StorageRefId;
                equip.Name = equipment.Name;
                equip.Serial = equipment.Serial;
                equip.IsSupportLine = equipment.IsSupportLine;
                equip.IsSupportInternet = equipment.IsSupportInternet;
                equip.Price = equipment.Price;
                equip.Type = equipment.Type;
                equip.UpdatedDate = equipment.UpdatedDate;
                context.EquipmentModels.Update(equip);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new EquipmentException("Can not Update Equipment ");
            }
        }
    }
}
