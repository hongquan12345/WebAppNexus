using Microsoft.AspNetCore.Mvc;

namespace NexusApp.Areas.Infrastructure.Controllers
{
    [Area("Infrastructure")]
    public class InfrastructureController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
