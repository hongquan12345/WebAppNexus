using Microsoft.AspNetCore.Mvc;

namespace NexusApp.Areas.Financial.Controllers
{
    [Area("Financial")]

    public class PaymentController : Controller
    {
        public IActionResult PaymentSuccess()
        {
            return View();
        }
        public IActionResult PaymentFailure()
        {
            return View();
        }
        public IActionResult PaymentCancel()
        {
            return View();

        }
    }
}
