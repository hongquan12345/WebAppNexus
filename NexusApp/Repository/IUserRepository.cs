using NexusApp.Areas.Customer.Models;

namespace NexusApp.Repository
{
    public interface IUserRepository
    {
        Task<CustomerModel> GetUserByEmail (string email);
       Task<CustomerModel> Update (CustomerModel customer);
        Task<CustomerModel> ChangePasswold(CustomerModel model);
    }
}
