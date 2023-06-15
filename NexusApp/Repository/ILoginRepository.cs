using NexusApp.Areas.Customer.Models;
using NexusApp.Areas.Employee.Models;
using NexusApp.ModelDTOs;

namespace NexusApp.Repository
{
    public interface ILoginRepository
    {
        Task<CustomerModel> CheckLogin(string Email);
         Task<EmployeeModel> CheckLoginAdmin(string Email);
        void Checkout();
    }
}
