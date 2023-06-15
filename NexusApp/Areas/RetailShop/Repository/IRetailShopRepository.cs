using NexusApp.Areas.RetailShop.Models;
using NexusApp.Areas.ServiceConnection.Models;

namespace NexusApp.Areas.RetailShop.Repository
{
    public interface IRetailShopRepository
    {
        Task<List<RetailShopModel>> GetAllRetailShop();
        Task<RetailShopModel> GetRetailShopByID(int id);
        Task AddRetailShop(RetailShopModel retailshop);
        Task UpdateRetailShop(RetailShopModel retailshop);
        Task DeleteRetailShop(int id);
    }
}
