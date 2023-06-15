using NexusApp.Areas.ServiceConnection.Models;

namespace NexusApp.Areas.Financial.Reposetory.ServiceConnection
{
    public interface IServiceConnectionRepository
    {
        Task<List<ServiceConnectionModel>> GetAllServiceConect();
        Task<List<ServiceConnectionModel>> GetServiceConectWithSubCon();

        Task<ServiceConnectionModel> GetServiceConectByID(int id);
        Task AddServiceConnection(ServiceConnectionModel serviceconnection);
        Task UpdateServiceConnection(ServiceConnectionModel serviceconnection);
        Task DeleteServiceConnection(int id);
    }
}
