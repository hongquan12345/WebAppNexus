using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusApp.Areas.Financial.Models.ModelsViews;
using NexusApp.Areas.Financial.Reposetory.Customer;
using NexusApp.Areas.Financial.Reposetory.Service;
using NexusApp.Areas.Financial.Reposetory.ServiceConnection;
using NexusApp.Areas.Financial.Reposetory.ServiceSub;
using NexusApp.Areas.ServiceConnection.Models;
using NexusApp.Data;
using NexusApp.Models;
using System.Diagnostics;

namespace NexusApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext datacontext;
        private readonly ICustomerRepository customers;
        private readonly IServiceRepository service;
        private readonly ISubServiceRepository subservice;
        private readonly IServiceConnectionRepository Sconservice;
        public readonly MailUtils mailUtils;        
        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext dbContext, IServiceRepository _service,
            ISubServiceRepository _subservice,
            IServiceConnectionRepository _Sconservice, 
            ICustomerRepository _customers, MailUtils mailUtils)
        {
            _logger = logger;
            datacontext = dbContext;
            service = _service;
            subservice = _subservice;
            Sconservice = _Sconservice;
            customers = _customers;
            this.mailUtils = mailUtils;
        }
        public async Task<IActionResult> Index()
        {
            var data = datacontext.serviceModels.Include(b=>b.SubServiceConnections).ToList();
            var dataSub = datacontext.subServiceConnectionModels.Include(b => b.ServiceModels).ToList();
            var ser = await Sconservice.GetServiceConectWithSubCon();
            if(ser!=null)
            {
                ViewBag.ServiceScon = ser;
            }
            ViewBag.Service = data;
            ViewBag.ServiceSub = dataSub;
            return View();
        }
        public async Task<IActionResult> GetSubSelection(int id)
        {
            if (id > 0)
            {
                List<SubServiceConnectionModel> subServiceOptions = await subservice.GetSubServiceBySconID(id);
                var data = new
                {
                    SubServiceOptions = subServiceOptions,
                };
                return Json(data);
            }
            return Json(null);
        }
        public async Task<IActionResult> GetServiceSelection(int id)
        {
            if (id > 0)
            {
                List<ServiceModel> sers = await service.GetServiceBySubID(id);
                var data = new
                {
                    services = sers,
                };
                return Json(data);
            }
            return Json(null);
        }
        [HttpPost]
        public async Task<IActionResult> CustomerRegister(CustomerRegister customerRegister)
        {      
            if (customerRegister != null)
            {
                await customers.AddRegiserCustomer(customerRegister);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}