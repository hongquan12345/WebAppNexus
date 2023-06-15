using Microsoft.EntityFrameworkCore;
using NexusApp.Areas.ServiceConnection.Models;
using NexusApp.Data;


namespace NexusApp.Areas.Financial.Reposetory.Service
{
    public class ServiceImp : IServiceRepository
    {
        private readonly ApplicationDbContext context;
        public ServiceImp(ApplicationDbContext _context)
        {
            context = _context;
        }
        public class ServiceException : Exception
        {
            public ServiceException(string message) : base(message) { }
        }
        public async Task AddService(ServiceModel service)
        {
            var ser = new ServiceModel();
            if (ser != null)
            {
                ser.Name = service.Name;
                ser.ServicePrice = service.ServicePrice;
                ser.Duration = service.Duration;
                ser.SubServiceConnectionRefId = service.SubServiceConnectionRefId;
                ser.CreatedDate = DateTime.Now;
                await context.serviceModels.AddAsync(ser);
                await context.SaveChangesAsync();
            }
        }
        public async Task DeleteService(int id)
        {
            var service = await context.serviceModels.FindAsync(id);
            if (service != null)
            {
                context.serviceModels.Remove(service);
                await context.SaveChangesAsync();
            }
        }
        public async Task<List<ServiceModel>> GetAllService()
        {
            var ser = await context.serviceModels.Include(s=>s.SubServiceConnections).ToListAsync();
            if (ser != null)
            {
                return ser;
            }
            else
            {
                throw new ServiceException("Can't Get List Service");
            }
        }
        public async Task<ServiceModel> GetServiceByID(int id)
        {
            var sers = await context.serviceModels.FindAsync(id);
            if (sers != null)
            {
                var sera = await context.serviceModels.
                Include(s => s.SubServiceConnections).
                ThenInclude(sc => sc.ServiceConnections).
                FirstOrDefaultAsync(c => c.ServiceId == sers.ServiceId);
                if (sera != null)
                {
                    return sera;
                }

                else
                {
                    throw new ServiceException(" Service by ID :" + id +"not Found !");
                }
            }
            else
            {
                throw new ServiceException("Can't Get Service by ID :"+ id);
            }    
        }       
        public async Task UpdateService(ServiceModel service)
        {
            var ser = await context.serviceModels.FindAsync(service.ServiceId);
            if (ser != null)
            {
                ser.Name = service.Name;
                ser.ServicePrice = service.ServicePrice;
                ser.Duration = service.Duration;
                ser.SubServiceConnectionRefId = service.SubServiceConnectionRefId;
                ser.UpdatedDate = DateTime.Now;
                context.serviceModels.Update(ser);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<ServiceModel>> GetServiceBySubID(int id)
        {
            var ser = await context.serviceModels.Where(s => s.SubServiceConnectionRefId == id).ToListAsync();
            if (ser != null)
            {
                return ser.ToList();
            }
            else
            {
                throw new ServiceException("Can't Get Service by SubID :" + id);
            }
        }
    }
}
