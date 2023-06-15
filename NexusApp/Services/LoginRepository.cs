using Microsoft.EntityFrameworkCore;
using NexusApp.Areas.Customer.Models;
using NexusApp.Areas.Employee.Models;
using NexusApp.Data;
using NexusApp.Repository;

namespace NexusApp.Services
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public LoginRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }
        public async Task<CustomerModel> CheckLogin(string Email)
        {
            var data = await _context.customerModels.FirstOrDefaultAsync(m => m.Email == Email);
            if (data == null)
            {
                return null;
            }
            else
            {
                var dataDb = (from x in _context.accountModels
                              where x.CustomerRefId == data.CustomerId
                              select x).ToList();
                if (dataDb.Count == 0)
                {
                    return null;
                }
                else
                {
                    if (data != null)
                    {
                        return data;
                    }
                    else
                    {
                        return null;
                    }
                }
            }



        }

        public async Task<EmployeeModel> CheckLoginAdmin(string Email)
        {
            var data = await _context.Employees.FirstOrDefaultAsync(b=>b.Email == Email);
            if (data != null)
            {
                return data;
            }
            return null;
        }

        public void Checkout()
        {
            if (_contextAccessor.HttpContext != null)
            {
                if (_contextAccessor.HttpContext.Session.GetString("Login") != null)
                {
                    _contextAccessor.HttpContext.Session.Clear();

                }
            }
        }
    }
}
