using NexusApp.Areas.Survey.Models;

namespace NexusApp.Areas.Financial.Reposetory.Install
{
    public interface InstallReposetory
    {
        Task<List<InstallModel>> GetAllInstall();
        Task<InstallModel> GetInstallById(int id);
        Task UpdateInstall(InstallModel installform);
    }
}
