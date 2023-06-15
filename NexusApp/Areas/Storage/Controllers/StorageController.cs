using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NexusApp.Areas.Financial;
using NexusApp.Areas.RetailShop.Models;
using NexusApp.Areas.RetailShop.Repository;
using NexusApp.Areas.Storage.Models;
using NexusApp.Areas.Storage.Repository.Storage;
using NexusApp.Data;
using X.PagedList;

namespace NexusApp.Areas.Storage.Controllers
{
    [Area("Storage")]
    public class StorageController : Controller
    {
        private ApplicationDbContext context;
        private IStorageRepository store;
        public StorageController(ApplicationDbContext _context, IStorageRepository _store)
        {
            context = _context;
            store = _store;
        }
        [CustomAuthorization("Admin", "Manager","Accountant")]
        public async Task<IActionResult> Index(string SearchName, int page = 1)
        {
            var storage = await store.GetAllStorage();
            if (storage != null)
            {
                if (!string.IsNullOrEmpty(SearchName))
                {
                    storage = storage.Where(e => e.Name.Contains(SearchName)).ToList();
                }
                page = page < 1 ? 1 : page;
                int pageSize = 3;
                IPagedList<StorageModel> paged = storage.ToPagedList(page, pageSize);
                return View(paged);
            }
            return View(storage);
        }
        [HttpGet]
        [CustomAuthorization("Admin", "Accountant")]
        public IActionResult Create()
        {
            var employee = context.Employees.ToList();
            ViewBag.noDepartxxy = new SelectList(employee, "EmployeeId", "Name");
            return View();
        }
        [HttpPost]
        [CustomAuthorization("Admin", "Accountant")]
        public async Task<IActionResult> Create(StorageModel storage)
        {
            try
            {
                var employee = context.Employees.ToList();
                ViewBag.noDepartxxy = new SelectList(employee, "EmployeeId", "Name");
                if (ModelState.IsValid)
                {
                    await store.AddStorage(storage);
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
            var employee = context.Employees.ToList();
            ViewBag.noDepartxxy = new SelectList(employee, "EmployeeId", "Name");
            var storage = await context.storageModels.FindAsync(id);
            return View(storage);
        }
        [HttpPost]
        [CustomAuthorization("Admin", "Accountant")]
        public async Task<IActionResult> EditOrDelete(StorageModel storage, string submit, int id)
        {
            try
            {
                var employee = context.Employees.ToList();
                ViewBag.noDepartxxy = new SelectList(employee, "EmployeeId", "Name");
                if (submit.Equals("Update"))
                {
                    await store.UpdateStorage(storage);
                    return RedirectToAction("Index");
                }
                else
                {
                    await store.DeleteStorage(id);
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
