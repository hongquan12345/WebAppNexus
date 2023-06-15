using Microsoft.EntityFrameworkCore;
using NexusApp.Areas.Employee.Models;
using NexusApp.Data;
using static NexusApp.Areas.RetailShop.Repository.RetailShopImp;

namespace NexusApp.Areas.Employee.Repository
{
    public class EmployeeImp : IEmployeeRepository
    {
        private readonly ApplicationDbContext context;
        public EmployeeImp(ApplicationDbContext _context)
        {
            context = _context;
        }
        public class EmployeeException : Exception
        {
            public EmployeeException(string message) : base(message) { }
        }
        public async Task AddEmployee(EmployeeModel employee)
        {
            if (employee != null)
            {
                employee.CreatedDate = DateTime.Now;
                await context.Employees.AddAsync(employee);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new EmployeeException("Can not Add Employee ");
            }
        }

        public async Task DeleteEmployee(int id)
        {
            var employee = await context.Employees.FindAsync(id);
            if (employee != null)
            {
                context.Employees.Remove(employee);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new EmployeeException("Can not Delete Employee with this ID");
            }
        }

        public async Task<List<EmployeeModel>> GetAllEmployee()
        {
            var employee = await context.Employees.Include(e => e.RetailShop).ToListAsync();
            if (employee != null)
            {
                return employee;
            }
            else
            {
                throw new EmployeeException("Can not get list Employee ");
            }
        }

        public async Task<EmployeeModel> GetEmployeeByID(int id)
        {
            var employee = await context.Employees.FindAsync(id);
            if (employee != null)
            {
                return employee;
            }
            else
            {
                throw new EmployeeException("Can not get Employee by ID");
            }
        }

        public async Task UpdateEmployee(EmployeeModel employee)
        {
            var emp  = await context.Employees.FindAsync(employee.EmployeeId);
            if (emp!= null)
            {
                emp.Name = employee.Name;
                emp.Address = employee.Address;
                emp.Email = employee.Email;
                emp.Phone = employee.Phone;
                emp.Password = employee.Password;
                emp.Role = employee.Role;
                emp.IsActive = employee.IsActive;
                emp.Position = employee.Position;
                emp.StartedDate = employee.StartedDate;
                emp.UpdatedDate = DateTime.Now;
                emp.RetaishopRefId = employee.RetaishopRefId;
                context.Employees.Update(emp);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new EmployeeException("Can not Update Employee ");
            }
        }

        public async Task<List<EmployeeModel>> GetEmployeewithrole(string role)
        {
            var employee = await context.Employees.Include(e => e.RetailShop).Where(c=>c.Role == role).ToListAsync();
            if (employee != null)
            {
                return employee;
            }
            else
            {
                throw new EmployeeException("Can not get list Employee ");
            }
        }
    }
}
