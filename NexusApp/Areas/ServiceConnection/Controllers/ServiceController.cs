using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NexusApp.Areas.Financial;
using NexusApp.Areas.Financial.Reposetory.Service;
using NexusApp.Areas.Financial.Reposetory.ServiceSub;
using NexusApp.Areas.ServiceConnection.Models;
using NexusApp.Areas.Storage.Models;
using NexusApp.Data;
using X.PagedList;

namespace NexusApp.Areas.ServiceConnection.Controllers
{
    [Area("ServiceConnection")]
    public class ServiceController : Controller
    {
        private ApplicationDbContext context;
        private IServiceRepository ser;
        public ServiceController(ApplicationDbContext _context, IServiceRepository _ser)
        {
            context = _context;
            ser = _ser;
        }
        [CustomAuthorization("Admin", "Manager", "Accountant")]
        public async Task<IActionResult> Index(string SearchName, int page = 1)
        {
            var service = await ser.GetAllService();
            if (service != null)
            {
                if (!string.IsNullOrEmpty(SearchName))
                {
                    service = service.Where(e => e.Name.Contains(SearchName)).ToList();
                }
                page = page < 1 ? 1 : page;
                int pageSize = 3;
                IPagedList<ServiceModel> paged = service.ToPagedList(page, pageSize);
                return View(paged);
            }
            return View(service);
        }
        [HttpGet]
        [CustomAuthorization("Admin", "Accountant")]
        public IActionResult Create()
        {
            var subservice = context.subServiceConnectionModels.ToList();
            ViewBag.noDepartxxy = new SelectList(subservice, "SubServiceConnectionId", "Name");
            return View();
        }
        [HttpPost]
        [CustomAuthorization("Admin", "Accountant")]
        public async Task<IActionResult> Create(ServiceModel service)
        {
            try
            {
                var subservice = context.subServiceConnectionModels.ToList();
                ViewBag.noDepartxxy = new SelectList(subservice, "SubServiceConnectionId", "Name");
                ModelState.Remove("Customers");
                ModelState.Remove("Connections");
                if (ModelState.IsValid)
                {
                    await ser.AddService(service);
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
            var subservice = context.subServiceConnectionModels.ToList();
            ViewBag.noDepartxxy = new SelectList(subservice, "SubServiceConnectionId", "Name");
            return View(context.serviceModels.Find(id));
        }
        [HttpPost]
        [CustomAuthorization("Admin", "Accountant")]
        public async Task<IActionResult> EditOrDelete(ServiceModel service, string submit, int id)
        {
            try
            {
                var subservice = context.subServiceConnectionModels.ToList();
                ViewBag.noDepartxxy = new SelectList(subservice, "SubServiceConnectionId", "Name");
                if (submit.Equals("Update"))
                {
                    await ser.UpdateService(service);
                    return RedirectToAction("Index");
                }
                else
                {
                    await ser.DeleteService(id);
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
