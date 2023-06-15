using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NexusApp.Areas.Financial;
using NexusApp.Areas.Financial.Reposetory.ServiceConnection;
using NexusApp.Areas.Financial.Reposetory.ServiceSub;
using NexusApp.Areas.ServiceConnection.Models;
using NexusApp.Data;
using X.PagedList;

namespace NexusApp.Areas.ServiceConnection.Controllers
{
    [Area("ServiceConnection")]
    public class SubServiceConnectionController : Controller
    {
        private ApplicationDbContext context;
        private ISubServiceRepository subser;
        public SubServiceConnectionController(ApplicationDbContext _context, ISubServiceRepository _subser)
        {
            context = _context;
            subser = _subser;
        }
        [CustomAuthorization("Admin", "Manager", "Accountant")]
        public async Task<IActionResult> Index(string SearchName, int page = 1)
        {
            var subservice = await subser.GetAllSubService();
            if (subservice != null)
            {
                if (!string.IsNullOrEmpty(SearchName))
                {
                    subservice = subservice.Where(e => e.Name.Contains(SearchName)).ToList();
                }
                page = page < 1 ? 1 : page;
                int pageSize = 3;
                IPagedList<SubServiceConnectionModel> paged = subservice.ToPagedList(page, pageSize);
                return View(paged);
            }
            return View(subservice);
        }
        [HttpGet]
        [CustomAuthorization("Admin", "Accountant")]
        public IActionResult Create()
        {
            var serviceconnect = context.serviceConnectionModels.ToList();
            ViewBag.noDepartxxy = new SelectList(serviceconnect, "ServiceConnectionId", "Name");
            return View();
        }
        [HttpPost]
        [CustomAuthorization("Admin", "Accountant")]
        public async Task<IActionResult> Create(SubServiceConnectionModel subserviceConnection)
        {
            try
            {
                var serviceconnect = context.serviceConnectionModels.ToList();
                ViewBag.noDepartxxy = new SelectList(serviceconnect, "ServiceConnectionId", "Name");
                if (ModelState.IsValid)
                {
                    await subser.AddSubServicer(subserviceConnection);
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
            var serviceconnect = context.serviceConnectionModels.ToList();
            ViewBag.noDepartxxy = new SelectList(serviceconnect, "ServiceConnectionId", "Name");
            return View(context.subServiceConnectionModels.Find(id));
        }
        [HttpPost]
        [CustomAuthorization("Admin", "Accountant")]
        public async Task<IActionResult> EditOrDelete(SubServiceConnectionModel subserviceConnection, string submit, int id)
        {
            try
            {
                var serviceconnect = context.serviceConnectionModels.ToList();
                ViewBag.noDepartxxy = new SelectList(serviceconnect, "ServiceConnectionId", "Name");
                if (submit.Equals("Update"))
                {
                    await subser.UpdateSubServicer(subserviceConnection);
                    return RedirectToAction("Index");
                }
                else
                {
                    await subser.DeleteSubServicer(id);
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
