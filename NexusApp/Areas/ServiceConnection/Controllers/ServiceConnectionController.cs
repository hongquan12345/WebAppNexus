using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NexusApp.Areas.Customer.Models;
using NexusApp.Areas.Financial;
using NexusApp.Areas.Financial.Reposetory.ServiceConnection;
using NexusApp.Areas.ServiceConnection.Models;
using NexusApp.Data;
using X.PagedList;

namespace NexusApp.Areas.ServiceConnection.Controllers
{
    [Area("ServiceConnection")]
    public class ServiceConnectionController : Controller
    {
        private ApplicationDbContext context;
        private IServiceConnectionRepository serviceCon;
        public ServiceConnectionController(ApplicationDbContext _context, IServiceConnectionRepository _serviceCon)
        {
            context = _context;
            serviceCon = _serviceCon;
        }
        [CustomAuthorization("Admin", "Manager", "Accountant")]
        public async Task<IActionResult> Index(string SearchName, int page = 1)
        {
            var serviceconnection = await serviceCon.GetAllServiceConect();
            if (serviceconnection != null)
            {
                if (!string.IsNullOrEmpty(SearchName))
                {
                    serviceconnection = serviceconnection.Where(e => e.Name.Contains(SearchName)).ToList();
                }
                page = page < 1 ? 1 : page;
                int pageSize = 3;
                IPagedList<ServiceConnectionModel> paged = serviceconnection.ToPagedList(page, pageSize);
                return View(paged);
            }
            return View(serviceconnection);
        }
        [HttpGet]
        [CustomAuthorization("Admin", "Accountant")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [CustomAuthorization("Admin", "Accountant")]
        public async Task<IActionResult> Create(ServiceConnectionModel serviceConnection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await serviceCon.AddServiceConnection(serviceConnection);

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
        [CustomAuthorization("Admin", "Accountant")]
        public async Task<IActionResult> EditOrDelete(int id)
        {
            var serviceconnection = await context.serviceConnectionModels.FindAsync(id);
            return View(serviceconnection);
        }
        [HttpPost]
        [CustomAuthorization("Admin", "Accountant")]
        public async Task<IActionResult> EditOrDelete(ServiceConnectionModel serviceConnection, string submit, int id)
        {
            try
            {
                if (submit.Equals("Update"))
                {
                    await serviceCon.UpdateServiceConnection(serviceConnection);
                    return RedirectToAction("Index");
                }
                else
                {
                    await serviceCon.DeleteServiceConnection(id);
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
