using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NexusApp.Areas.Admin.Models;
using NexusApp.Areas.Financial;
using NexusApp.Areas.Financial.Reposetory.Payment;
using NexusApp.ModelDTOs;
using System.Linq;
using static NexusApp.ModelDTOs.DashboardViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NexusApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IPaymentReposetory paymentReposetory;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DashboardController(IPaymentReposetory _paymentReposetory, IHttpContextAccessor _httpContextAccessor)
        {
            paymentReposetory = _paymentReposetory;
            httpContextAccessor = _httpContextAccessor;
        }
        [CustomAuthorization("Admin", "Manager", "Technical", "Accountant")]

        public async Task<IActionResult> Index()
        {
            HttpContext httpContext = httpContextAccessor.HttpContext;
            ISession session = httpContext.Session;

            string transactionsJson = session.GetString("Transactions");

            List<TransactionViewModel> transactions = string.IsNullOrEmpty(transactionsJson) ?
            new List<TransactionViewModel>() :
            JsonConvert.DeserializeObject<List<TransactionViewModel>>(transactionsJson);


            ViewData["Title"] = "Admin Page";
            var dashboardViewModel = new DashboardViewModel();
            dashboardViewModel.Transactions = transactions;

            dashboardViewModel.MonthlyData = await paymentReposetory.DashBoard("month");
            dashboardViewModel.LastMonthData = await paymentReposetory.DashBoard("lastmonth");
            dashboardViewModel.YearlyData = await paymentReposetory.DashBoard("year");
            dashboardViewModel.LastYearData = await paymentReposetory.DashBoard("lastyear");

            if (dashboardViewModel != null)
            {
                var currentYearValue = dashboardViewModel.YearlyData.Values.Sum();
                var previousYearValue = dashboardViewModel.LastYearData.Values.Sum();
                var currentmonthValue = dashboardViewModel.MonthlyData.Values.Sum();
                var previousmonthValue = dashboardViewModel.LastMonthData.Values.Sum();

                if (currentYearValue >0 || previousYearValue >0)
                {
                    decimal differenceYeah = currentYearValue - previousYearValue;
                    decimal percentDifference = 0;
                    if (previousYearValue != 0)
                    {
                        percentDifference = (differenceYeah / previousYearValue) * 100;
                    }
                    else
                    {
                        percentDifference = 0;
                    }
                    percentDifference = Math.Round(percentDifference, 2);
                    dashboardViewModel.YearDifference = percentDifference;
                }
                if (currentmonthValue>0 || previousmonthValue >0)
                {
                    decimal differenceMount = currentmonthValue - previousmonthValue;
                    decimal percentDifferencemonth = 0;
                    if (previousmonthValue != 0)
                    {
                        percentDifferencemonth = (differenceMount / previousmonthValue) * 100;
                    }
                    else
                    {
                        percentDifferencemonth = 0;
                    }
                    percentDifferencemonth = Math.Round(percentDifferencemonth, 2);
                    dashboardViewModel.MonthDifference = percentDifferencemonth;
                }
            }
            return View(dashboardViewModel);
        }
        [HttpGet]
        public async Task <IActionResult> GetChartData()
        {
            var data = await paymentReposetory.GetMonthlyData();
            return Json(data);
        }
    }
}
