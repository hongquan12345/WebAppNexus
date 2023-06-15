using NexusApp.Areas.Customer.Models;
using NexusApp.Areas.ServiceConnection.Models;

namespace NexusApp.Areas.Financial.Reposetory.ServiceSub
{
    public interface ISubServiceRepository
    {
        Task<List<SubServiceConnectionModel>> GetAllSubService();
        Task<List<SubServiceConnectionModel>> GetSubServiceBySconID(int id);
        Task AddSubServicer(SubServiceConnectionModel subservice);
        Task UpdateSubServicer(SubServiceConnectionModel subservice);
        Task DeleteSubServicer(int id);
    }
}
