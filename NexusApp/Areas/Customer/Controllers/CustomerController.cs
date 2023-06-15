using Microsoft.AspNetCore.Mvc;

namespace NexusApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
