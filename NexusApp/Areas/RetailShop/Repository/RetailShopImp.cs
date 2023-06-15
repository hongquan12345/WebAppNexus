using Microsoft.EntityFrameworkCore;
using NexusApp.Areas.RetailShop.Models;
using NexusApp.Data;
using static NNexusApp.Areas.Financial.Reposetory.ServiceConnection.ServiceConnectImp;

namespace NexusApp.Areas.RetailShop.Repository
{
    public class RetailShopImp : IRetailShopRepository
    {
        private readonly ApplicationDbContext context;
        public RetailShopImp(ApplicationDbContext _context)
        {
            context = _context;
        }
        public class RetailShopException : Exception
        {
            public RetailShopException(string message) : base(message) { }
        }
        public async Task AddRetailShop(RetailShopModel retailshop)
        {
            if (retailshop != null)
            {
                retailshop.CreatedDate = DateTime.Now;
                await context.RetailShop.AddAsync(retailshop);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new RetailShopException("Can not Add RetailShop ");
            }
        }

        public async Task DeleteRetailShop(int id)
        {
            var retailshop = await context.RetailShop.FindAsync(id);
            if (retailshop != null)
            {
                context.RetailShop.Remove(retailshop);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new RetailShopException("Can not Delete RetailShop with this ID");
            }
        }

        public async Task<List<RetailShopModel>> GetAllRetailShop()
        {

            var retailshop = await context.RetailShop.Include(e=>e.Employees).ToListAsync();
            if (retailshop != null)
            {
                return retailshop;
            }
            else
            {
                throw new RetailShopException("Can not get list RetailShop ");
            }
        }

        public async Task<RetailShopModel> GetRetailShopByID(int id)
        {
            var retailshop = await context.RetailShop.FindAsync(id);
            if (retailshop != null)
            {
                return retailshop;
            }
            else
            {
                throw new RetailShopException("Can not get RetailShop by ID");
            }
        }

        public async Task UpdateRetailShop(RetailShopModel retailshop)
        {
            var retail = await context.RetailShop.FindAsync(retailshop.RetailShopId);
            if (retail != null)
            {
                retail.Name = retailshop.Name;
                retail.Email = retailshop.Email;
                retail.Phone = retailshop.Phone;
                retail.UpdatedDate = DateTime.Now;
                context.RetailShop.Update(retail);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new RetailShopException("Can not Update RetailShop ");
            }
        }
    }
}
