using NexusApp.Areas.Customer.Models;

namespace NexusApp.Areas.Financial.Reposetory.Account
{
    public interface IAccountReposetory
    {
        Task<List<AccountModel>> GetAllAcount();
        Task<AccountModel> GetAccountByID(int id);
        Task AddAccount(AccountModel account);
        Task UpdateAccount(AccountModel account);
        Task DeleteAccount(int id);
    }
}
