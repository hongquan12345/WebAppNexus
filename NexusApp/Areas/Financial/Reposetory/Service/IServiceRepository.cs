using NexusApp.Areas.Customer.Models;
using NexusApp.Areas.ServiceConnection.Models;

namespace NexusApp.Areas.Financial.Reposetory.Service
{
    public interface IServiceRepository
    {
        Task<List<ServiceModel>> GetAllService();
        Task<ServiceModel> GetServiceByID(int id);
        Task<List<ServiceModel>> GetServiceBySubID(int id);
        Task AddService(ServiceModel service);
        Task UpdateService(ServiceModel service);
        Task DeleteService(int id);
    }
}
