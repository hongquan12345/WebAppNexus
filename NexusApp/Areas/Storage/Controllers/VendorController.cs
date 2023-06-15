using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NexusApp.Areas.Financial;
using NexusApp.Areas.Storage.Models;
using NexusApp.Areas.Storage.Repository.Storage;
using NexusApp.Areas.Storage.Repository.Vendor;
using NexusApp.Data;
using X.PagedList;

namespace NexusApp.Areas.Storage.Controllers
{
    [Area("Storage")]
    public class VendorController : Controller
    {
        private ApplicationDbContext context;
        private IVendorRepository ven;
        public VendorController(ApplicationDbContext _context, IVendorRepository _ven)
        {
            context = _context;
            ven = _ven;
        }
        [CustomAuthorization("Admin", "Manager","Accountant")]
        public async Task<IActionResult> Index(string SearchName, int page = 1)
        {
            var vendor = await ven.GetAllVendor();
            if (vendor != null)
            {
                if (!string.IsNullOrEmpty(SearchName))
                {
                    vendor = vendor.Where(e => e.Name.Contains(SearchName)).ToList();
                }
                page = page < 1 ? 1 : page;
                int pageSize = 3;
                IPagedList<VendorModel> paged = vendor.ToPagedList(page, pageSize);
                return View(paged);
            }
            return View(vendor);
        }
        [HttpGet]
        [CustomAuthorization("Admin", "Accountant")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [CustomAuthorization("Admin", "Accountant")]
        public async Task<IActionResult> Create(VendorModel vendor)
        {
            try
            {
                ModelState.Remove("Vendor_Equipment");
                if (ModelState.IsValid)
                {
                    await ven.AddVendor(vendor);
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
            var vendor = await context.vendorModels.FindAsync(id);
            return View(vendor);
        }
        [HttpPost]
        [CustomAuthorization("Admin", "Accountant")]
        public async Task<IActionResult> EditOrDelete(VendorModel vendor, string submit, int id)
        {
            try
            {
                if (submit.Equals("Update"))
                {
                    await ven.UpdateVendor(vendor);
                    return RedirectToAction("Index");
                }
                else
                {
                    await ven.DeleteVendor(id);
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
