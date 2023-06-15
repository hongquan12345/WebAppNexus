using Microsoft.EntityFrameworkCore;
using NexusApp.Areas.Financial.Reposetory.ServiceSub;
using NexusApp.Areas.ServiceConnection.Models;
using NexusApp.Data;

namespace NNexusApp.Areas.Financial.Reposetory.ServiceSub
{
    public class SubServiceImp : ISubServiceRepository
    {
        private readonly ApplicationDbContext context;
        public SubServiceImp(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task AddSubServicer(SubServiceConnectionModel subservice)
        {
            if (subservice != null)
            {
                subservice.CreatedDate = DateTime.Now;
                await context.subServiceConnectionModels.AddAsync(subservice);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteSubServicer(int id)
        {
            var subser = await context.subServiceConnectionModels.FindAsync(id);
            if (subser != null)
            {
                context.subServiceConnectionModels.Remove(subser);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<SubServiceConnectionModel>> GetAllSubService()
        {
            var subser = await context.subServiceConnectionModels.Include(s=>s.ServiceConnections).ToListAsync();
            if (subser != null)
            {
                return subser;
            }
            return null;
        }
        public async Task<List<SubServiceConnectionModel>> GetSubServiceBySconID(int id)
        {

            var subser = await context.subServiceConnectionModels.Where(s => s.ServiceConnectionRefId == id).ToListAsync();
            if (subser != null)
            {
                return subser.ToList();
            }
            return null;
        }

        public async Task UpdateSubServicer(SubServiceConnectionModel subservice)
        {
            var subser = await context.subServiceConnectionModels.FindAsync(subservice.SubServiceConnectionId);
            if (subser != null)
            {
                subser.Name = subservice.Name;
                subser.ServiceConnectionRefId = subservice.ServiceConnectionRefId;
                subser.UpdatedDate = DateTime.Now;
                context.subServiceConnectionModels.Update(subser);
                await context.SaveChangesAsync();
            }

        }
    }
}
