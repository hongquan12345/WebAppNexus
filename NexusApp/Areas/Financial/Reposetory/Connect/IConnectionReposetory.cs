using NexusApp.Areas.Financial.Models;

namespace NexusApp.Areas.Financial.Reposetory.Connect
{
    public interface IConnectionReposetory
    {
        Task<List<ConnectionModel>> GetAllConnect();
        Task<ConnectionModel> GetConnectbyId(int id);
        Task UpdateConnection(ConnectionModel connection,int? time);
        Task CreateConnection(ConnectionModel connection);
        Task DeteleConnection(int id);
    }
}
