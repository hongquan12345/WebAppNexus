using NexusApp.Areas.Storage.Models;

namespace NexusApp.Areas.Storage.Repository.Equipment
{
    public interface IEquipmentRepository
    {
        Task<List<EquipmentModel>> GetAllEquipment();
        Task<EquipmentModel> GetEquipmentByID(int id);
        Task AddEquipment(EquipmentModel equipment);
        Task UpdateEquipment(EquipmentModel equipment);
        Task DeleteEquipment(int id);
    }
}
