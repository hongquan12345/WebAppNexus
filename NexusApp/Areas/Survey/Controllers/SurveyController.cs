using Microsoft.AspNetCore.Mvc;
using NexusApp.Areas.Survey.Models;
using NexusApp.Data;

namespace NexusApp.Areas.Survey.Controllers
{
    [Area("Survey")]
    public class SurveyController : Controller
    {
        private ApplicationDbContext context;
        public SurveyController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var surveys = context.surveyModels.ToList();
            return View(surveys);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(SurveyModel survey)
        {

            context.surveyModels.Add(survey);
            survey.CreatedDate = DateTime.Now;
            survey.UpdatedDate = DateTime.Now;
            context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
