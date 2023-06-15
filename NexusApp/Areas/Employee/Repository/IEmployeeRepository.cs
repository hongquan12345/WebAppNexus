using NexusApp.Areas.Employee.Models;
using NexusApp.Areas.RetailShop.Models;

namespace NexusApp.Areas.Employee.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<EmployeeModel>> GetAllEmployee();
        Task<EmployeeModel> GetEmployeeByID(int id);
        Task<List<EmployeeModel>> GetEmployeewithrole(string role);
        Task AddEmployee(EmployeeModel employee);
        Task UpdateEmployee(EmployeeModel employee);
        Task DeleteEmployee(int id);
    }
}
