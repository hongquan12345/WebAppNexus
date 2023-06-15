using NexusApp.Areas.RetailShop.Models;
using NexusApp.Areas.Storage.Models;

namespace NexusApp.Areas.Storage.Repository.Storage
{
    public interface IStorageRepository
    {
        Task<List<StorageModel>> GetAllStorage();
        Task<StorageModel> GetStorageByID(int id);
        Task AddStorage(StorageModel storage);
        Task UpdateStorage(StorageModel storage);
        Task DeleteStorage(int id);
    }
}
