using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NexusApp.Areas.Customer.Models;
using NexusApp.Areas.Financial;
using NexusApp.Areas.Storage.Models;
using NexusApp.Areas.Storage.Repository.Equipment;
using NexusApp.Areas.Storage.Repository.Vendor;
using NexusApp.Data;
using System.Linq;
using X.PagedList;

namespace NexusApp.Areas.Storage.Controllers
{
    [Area("Storage")]
    public class EquipmentController : Controller
    {
        private ApplicationDbContext context;
        private IEquipmentRepository equip;
        public EquipmentController(ApplicationDbContext _context, IEquipmentRepository _equip)
        {
            context = _context;
            equip = _equip;
        }
        [CustomAuthorization("Admin", "Manager","Accountant")]
        public async Task<IActionResult> Index(string SearchName,int? SearchSerial, int page = 1)
        {
            var equiment = await equip.GetAllEquipment();
            if (equiment != null)
            {
                if (!string.IsNullOrEmpty(SearchName))
                {
                    equiment = equiment.Where(e => e.Name.Contains(SearchName)).ToList();
                }
                if (SearchSerial.HasValue)
                {
                    equiment = equiment.Where(e => e.Serial.ToString().Contains(SearchSerial.ToString())).ToList();
                }
                page = page < 1 ? 1 : page;
                int pageSize = 3;
                IPagedList<EquipmentModel> paged = equiment.ToPagedList(page, pageSize);
                return View(paged);
            }
            return View(equiment);
        }
        [HttpGet]
        [CustomAuthorization("Admin", "Accountant")]
        public IActionResult Create()
        {
            var storage = context.storageModels.ToList();
            ViewBag.noDepartxxy = new SelectList(storage, "StorageId", "Name");
            return View();
        }
        [HttpPost]
        [CustomAuthorization("Admin","Accountant")]
        public async Task<IActionResult> Create(EquipmentModel equipment)
        {
            try
            {
                var storage = context.storageModels.ToList();
                ViewBag.noDepartxxy = new SelectList(storage, "StorageId", "Name");
                if (ModelState.IsValid)
                {
                    await equip.AddEquipment(equipment);
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
            var storage = context.storageModels.ToList();
            ViewBag.noDepartxxy = new SelectList(storage, "StorageId", "Name");
            var equipment = await context.EquipmentModels.FindAsync(id);
            return View(equipment);
        }
        [HttpPost]
        [CustomAuthorization("Admin","Accountant")]
        public async Task<IActionResult> EditOrDelete(EquipmentModel equipment, string submit, int id)
        {
            try
            {
                var storage = context.storageModels.ToList();
                ViewBag.noDepartxxy = new SelectList(storage, "StorageId", "Name");
                if (submit.Equals("Update"))
                {
                    await equip.UpdateEquipment(equipment);
                    return RedirectToAction("Index");
                }
                else
                {
                    await equip.DeleteEquipment(id);
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
