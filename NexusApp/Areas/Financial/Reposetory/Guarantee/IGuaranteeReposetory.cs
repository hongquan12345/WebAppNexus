using NexusApp.Areas.Survey.Models;

namespace NexusApp.Areas.Financial.Reposetory.Guarantee
{
    public interface IGuaranteeReposetory
    {
        Task<List<GuaranteeModel>> GetAllGuarantee();
        Task<GuaranteeModel> GetGuaranteeById(int id);
        Task UpdateGuarantee(GuaranteeModel guaranteeform);
        Task CreateGuarantee(GuaranteeModel guaranteeform);
    }
}
