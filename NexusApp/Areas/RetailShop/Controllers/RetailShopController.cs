using Azure;
using Microsoft.AspNetCore.Mvc;
using NexusApp.Areas.Financial;
using NexusApp.Areas.Financial.Reposetory.ServiceConnection;
using NexusApp.Areas.RetailShop.Models;
using NexusApp.Areas.RetailShop.Repository;
using NexusApp.Areas.ServiceConnection.Models;
using NexusApp.Areas.Storage.Models;
using NexusApp.Data;
using X.PagedList;

namespace NexusApp.Areas.RetailShop.Controllers
{
    [Area("RetailShop")]
    public class RetailShopController : Controller
    {
        private ApplicationDbContext context;
        private IRetailShopRepository retail;
        public RetailShopController(ApplicationDbContext _context, IRetailShopRepository _retail)
        {
            context = _context;
            retail = _retail;
        }
        [CustomAuthorization("Admin", "Manager")]
        public async Task<IActionResult> Index(string SearchName, int page = 1)
        {
            var retailshop = await retail.GetAllRetailShop();
            if (retailshop != null)
            {
                if (!string.IsNullOrEmpty(SearchName))
                {
                    retailshop = retailshop.Where(e => e.Name.Contains(SearchName)).ToList();
                }
                page = page < 1 ? 1 : page;
                int pageSize = 3;
                IPagedList<RetailShopModel> paged = retailshop.ToPagedList(page, pageSize);
                return View(paged);
            }
            return View(retailshop);
        }
        [HttpGet]
        [CustomAuthorization("Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [CustomAuthorization("Admin")]
        public async Task<IActionResult> Create(RetailShopModel retailshop)
        {
            try
            {
                ModelState.Remove("Employees");
                if (ModelState.IsValid)
                {
                    await retail.AddRetailShop(retailshop);
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
        [CustomAuthorization("Admin")]
        public async Task<IActionResult> EditOrDelete(int id)
        {
            var retailshop = await context.RetailShop.FindAsync(id);
            return View(retailshop);
        }
        [HttpPost]
        [CustomAuthorization("Admin")]
        public async Task<IActionResult> EditOrDelete(RetailShopModel retailshop, string submit, int id)
        {
            try
            {
                if (submit.Equals("Update"))
                {
                    await retail.UpdateRetailShop(retailshop);
                    return RedirectToAction("Index");
                }
                else
                {
                    await retail.DeleteRetailShop(id);
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
