using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NexusApp.Areas.Financial;
using NexusApp.Areas.Storage.Models;
using NexusApp.Areas.Storage.Repository.Vendor;
using NexusApp.Areas.Storage.Repository.VendorEquipment;
using NexusApp.Data;
using System.Drawing;
using X.PagedList;

namespace NexusApp.Areas.Storage.Controllers
{
    [Area("Storage")]
    public class VendorEquipmentController : Controller
    {
        private ApplicationDbContext context;
        private IVendorEquipmentRepository vendorequip;
        public VendorEquipmentController(ApplicationDbContext _context, IVendorEquipmentRepository _vendorequip)
        {
            context = _context;
            vendorequip = _vendorequip;
        }
        [CustomAuthorization("Admin", "Manager","Accountant")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var vendorequipment = await vendorequip.GetAllVendorEquipment();
            if(vendorequipment != null)
            {
                page = page < 1 ? 1 : page;
                int pageSize = 3;
                IPagedList<Vendor_Equipment> paged = vendorequipment.ToPagedList(page, pageSize);
                return View(paged);
            }
            return View(vendorequipment);
        }
        [HttpGet]
        [CustomAuthorization("Admin", "Accountant")]
        public IActionResult Create()
        {
            var vendor = context.vendorModels.ToList();
            ViewBag.noDepartxxy = new SelectList(vendor, "VendorId", "Name");
            var equipment = context.EquipmentModels.ToList();
            ViewBag.noDepartxy = new SelectList(equipment, "EquipmentId", "Name");
            return View();
        }
        [HttpPost]
        [CustomAuthorization("Admin", "Accountant")]
        public async Task<IActionResult> Create(Vendor_Equipment vendorequipments)
        {
            try
            {
                var vendor = context.vendorModels.ToList();
                ViewBag.noDepartxxy = new SelectList(vendor, "VendorId", "Name");
                var equipment = context.EquipmentModels.ToList();
                ViewBag.noDepartxy = new SelectList(equipment, "EquipmentId", "Name");
                if (ModelState.IsValid)
                {
                    await vendorequip.AddVendorEquipment(vendorequipments);
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
            var vendor = context.vendorModels.ToList();
            ViewBag.noDepartxxy = new SelectList(vendor, "VendorId", "Name");
            var equipment = context.EquipmentModels.ToList();
            ViewBag.noDepartxy = new SelectList(equipment, "EquipmentId", "Name");
            var vendorequipment = await context.Vendor_Equipments.FindAsync(id);
            return View(vendorequipment);
        }
        [HttpPost]
        [CustomAuthorization("Admin", "Accountant")]
        public async Task<IActionResult> EditOrDelete(Vendor_Equipment vendorequipments, string submit, int id)
        {
            try
            {
                var vendor = context.vendorModels.ToList();
                ViewBag.noDepartxxy = new SelectList(vendor, "VendorId", "Name");
                var equipment = context.EquipmentModels.ToList();
                ViewBag.noDepartxy = new SelectList(equipment, "EquipmentId", "Name");
                if (submit.Equals("Update"))
                {
                    await vendorequip.UpdateVendorEquipment(vendorequipments);
                    return RedirectToAction("Index");
                }
                else
                {
                    await vendorequip.DeleteVendorEquipment(id);
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
