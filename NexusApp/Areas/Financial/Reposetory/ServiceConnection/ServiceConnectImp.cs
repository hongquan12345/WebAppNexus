using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NexusApp.Areas.Financial.Reposetory.ServiceConnection;
using NexusApp.Areas.ServiceConnection.Models;
using NexusApp.Data;
using static NexusApp.Areas.Financial.Reposetory.Customer.CustomerServiceImp;

namespace NNexusApp.Areas.Financial.Reposetory.ServiceConnection
{
    public class ServiceConnectImp : IServiceConnectionRepository
    {
        private readonly ApplicationDbContext context;
        public ServiceConnectImp(ApplicationDbContext _context)
        {
            context = _context;
        }
        public class ServiceSconException : Exception
        {
            public ServiceSconException(string message) : base(message) { }
        }
        public async Task<List<ServiceConnectionModel>> GetAllServiceConect()
        {
            var serCon = await context.serviceConnectionModels.ToListAsync();
            if (serCon != null)
            {
                return serCon;
            }
            else
            {
                throw new ServiceSconException("Can not get list ServiceConect ");
            }
        }
        public async Task<ServiceConnectionModel> GetServiceConectByID(int id)
        {
            var serCon = await context.serviceConnectionModels.FindAsync(id);
            if (serCon != null)
            {
                return serCon;
            }
            else
            {
                throw new ServiceSconException("Can not get ServiceConect by ID");
            }
        }

        public async Task AddServiceConnection(ServiceConnectionModel serviceconnection)
        {
            if (serviceconnection != null)
            {
                serviceconnection.CreatedDate = DateTime.Now;
                await context.serviceConnectionModels.AddAsync(serviceconnection);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new ServiceSconException("Can not Add ServiceConnect ");
            }
        }

        public async Task UpdateServiceConnection(ServiceConnectionModel serviceconnection)
        {
            var servicecon = await context.serviceConnectionModels.FindAsync(serviceconnection.ServiceConnectionId);
            if (servicecon != null)
            {
                servicecon.Name = serviceconnection.Name;
                servicecon.Deposit = serviceconnection.Deposit;
                servicecon.UpdatedDate = DateTime.Now;
                context.serviceConnectionModels.Update(servicecon);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new ServiceSconException("Can not Update ServiceConnect ");
            }
        }
        public async Task DeleteServiceConnection(int id)
        {
            var servicecon = await context.serviceConnectionModels.FindAsync(id);
            if (servicecon != null)
            {
                context.serviceConnectionModels.Remove(servicecon);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new ServiceSconException("Can not Delete ServiceConnect with this ID");
            }
        }

        public async Task<List<ServiceConnectionModel>> GetServiceConectWithSubCon()
        {
            var serCon = await context.serviceConnectionModels.Include(sb=>sb.SubServiceConnectionModels).ToListAsync();
            if (serCon != null)
            {
                return serCon;
            }
            else
            {
                throw new ServiceSconException("Can not get list ServiceConect ");
            }
        }
    }
}
