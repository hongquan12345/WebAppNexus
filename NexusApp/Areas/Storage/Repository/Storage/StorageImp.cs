using Microsoft.EntityFrameworkCore;
using NexusApp.Areas.Storage.Models;
using NexusApp.Data;
using static NexusApp.Areas.RetailShop.Repository.RetailShopImp;

namespace NexusApp.Areas.Storage.Repository.Storage
{
    public class StorageImp : IStorageRepository
    {
        private readonly ApplicationDbContext context;
        public StorageImp(ApplicationDbContext _context)
        {
            context = _context;
        }
        public class StorageException : Exception
        {
            public StorageException(string message) : base(message) { }
        }
        public async Task AddStorage(StorageModel storage)
        {
            if (storage != null)
            {
                storage.CreatedDate = DateTime.Now;
                await context.storageModels.AddAsync(storage);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new StorageException("Can not Add Storage ");
            }
        }

        public async Task DeleteStorage(int id)
        {
            var storage = await context.storageModels.FindAsync(id);
            if (storage != null)
            {
                context.storageModels.Remove(storage);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new StorageException("Can not Delete Storage with this ID");
            }
        }

        public async Task<List<StorageModel>> GetAllStorage()
        {
            var storage = await context.storageModels.Include(e=>e.Employee).ToListAsync();
            if (storage != null)
            {
                return storage;
            }
            else
            {
                throw new StorageException("Can not get list Storage ");
            }
        }

        public async Task<StorageModel> GetStorageByID(int id)
        {
            var storage = await context.storageModels.FindAsync(id);
            if (storage != null)
            {
                return storage;
            }
            else
            {
                throw new StorageException("Can not get Storage by ID");
            }
        }

        public async Task UpdateStorage(StorageModel storage)
        {
            var store = await context.storageModels.FindAsync(storage.StorageId);

            if (store != null)
            {
                store.Name = storage.Name;
                store.Location = storage.Location;
                store.EmployeeRefId = storage.EmployeeRefId;
                store.UpdatedDate = DateTime.Now;
                context.storageModels.Update(store);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new StorageException("Can not Update Storage ");
            }
        }
    }
}
