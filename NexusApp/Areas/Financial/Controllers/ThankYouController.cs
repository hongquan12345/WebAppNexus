using Microsoft.AspNetCore.Mvc;

namespace NexusApp.Areas.Financial.Controllers
{
    [Area("Financial")]

    public class ThankYouController : Controller
    {
        public IActionResult ThankYou()
        {
            return View();
        }
        public IActionResult RegisterNew()
        {
            return View();
        }
        public IActionResult RegisterUpdate()
        {
            return View();
        }
    }
}
