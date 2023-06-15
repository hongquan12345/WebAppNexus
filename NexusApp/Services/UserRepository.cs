using Microsoft.EntityFrameworkCore;
using NexusApp.Areas.Customer.Models;
using NexusApp.Data;
using NexusApp.Repository;

namespace NexusApp.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerModel> ChangePasswold(CustomerModel model)
        {

                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return model;
 
        }

        public async Task<CustomerModel> GetUserByEmail(string email)
        {
            var data = await _context.customerModels.Include(b => b.Accounts).Where(a => a.RegistrationStatus == true).FirstOrDefaultAsync(m => m.Email == email);
            if (data != null)
            {
                return data;
            }
            else
            {
                return null;
            }
        }

        public async Task<CustomerModel> Update(CustomerModel customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return customer;
        }
    }
}
