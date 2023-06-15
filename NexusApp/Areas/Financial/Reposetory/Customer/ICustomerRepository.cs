using NexusApp.Areas.Customer.Models;

namespace NexusApp.Areas.Financial.Reposetory.Customer
{
    public interface ICustomerRepository
    {
        Task <List<CustomerModel>> GetAllCustomer();
        Task<CustomerModel> GetCustomerByID(int id);
        Task AddCustomer(CustomerModel customer);
        Task AddRegiserCustomer(dynamic customer);
        Task UpdateCustomer(CustomerModel cusmodelform,int EmpID);
        Task DeleteCustomer(int id);
        Task<List<CustomerModel>> FindCustomerByScon(int id);
    }
}
