using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NexusApp.Areas.Employee.Models;
using NexusApp.Areas.Employee.Repository;
using NexusApp.Areas.Financial;
using NexusApp.Areas.RetailShop.Models;
using NexusApp.Areas.RetailShop.Repository;
using NexusApp.Constants;
using NexusApp.Data;
using PayPal.Api;
using System.Data;
using X.PagedList;
namespace NexusApp.Areas.Employee.Controllers
{

    [Area("Employee")]
    public class EmployeeController : Controller
    {

        private ApplicationDbContext context;
        private IEmployeeRepository emp;
        public EmployeeController(ApplicationDbContext _context, IEmployeeRepository _emp)
        {
            context = _context;
            emp = _emp;         
        }

        //AllowAnonymous là ai cũng vào dc, không dùng chung với Authorize
       /* [AllowAnonymous]*/
        [CustomAuthorization("Admin","Manager")]
        public async Task<IActionResult> Index(string SearchName, int page = 1)
        {
            var employee = await emp.GetAllEmployee();
            if (employee != null)
            {
                if (!string.IsNullOrEmpty(SearchName))
                {
                    employee = employee.Where(e => e.Name.Contains(SearchName)).ToList();
                }
                page = page < 1 ? 1 : page;
                int pageSize = 3;
                IPagedList<EmployeeModel> paged = employee.ToPagedList(page, pageSize);
                return View(paged);
            }
            return View(employee);
        }
        [HttpGet]
        [CustomAuthorization("Admin", "Manager")]
        public IActionResult Create()
        {
            var retailshop = context.RetailShop.ToList();
            ViewBag.noDepartxxy = new SelectList(retailshop, "RetailShopId", "Name");
            return View();
        }
        [HttpPost]
        [CustomAuthorization("Admin", "Manager")]
        public async Task<IActionResult> Create(EmployeeModel employee)
        {
            try
            {
                //dòng này chi ?
                var retailshop = context.RetailShop.ToList();
                ViewBag.noDepartxxy = new SelectList(retailshop, "RetailShopId", "Name");
                ModelState.Remove("Storages");
                ModelState.Remove("Surveys");
                ModelState.Remove("Guarantees");
                ModelState.Remove("OrderDetails");
                ModelState.Remove("RetailShop");
                if (ModelState.IsValid)
                {
                    await emp.AddEmployee(employee);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }

        [HttpGet]
        [CustomAuthorization("Admin", "Manager")]
        public async Task<IActionResult> EditOrDelete(int id)
        {
            var retailshop = context.RetailShop.ToList();
            ViewBag.noDepartxxy = new SelectList(retailshop, "RetailShopId", "Name");
            var employee = await context.Employees.FindAsync(id);
            return View(employee);
        }
        [HttpPost]
        [CustomAuthorization("Admin", "Manager")]
        public async Task<IActionResult> EditOrDelete(EmployeeModel employee, string submit, int id)
        {
            try
            {
                var retailshop = context.RetailShop.ToList();
                ViewBag.noDepartxxy = new SelectList(retailshop, "RetailShopId", "Name");
                if (submit.Equals("Update"))
                {
                    await emp.UpdateEmployee(employee);
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }
    }
}
