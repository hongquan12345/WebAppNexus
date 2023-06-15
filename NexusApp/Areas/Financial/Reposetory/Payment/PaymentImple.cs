using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Newtonsoft.Json;
using NexusApp.Areas.Financial.Models;
using NexusApp.Data;
using NexusApp.ModelDTOs;
using PayPal.Api;
using static NexusApp.ModelDTOs.DashboardViewModel;

namespace NexusApp.Areas.Financial.Reposetory.Payment
{
    public class PaymentImple : IPaymentReposetory
    {
        private readonly ApplicationDbContext context;
        public readonly MailUtils mailUtils;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public PaymentImple(ApplicationDbContext _context, MailUtils _mailUtils, IWebHostEnvironment _webHostEnvironment, IHttpContextAccessor _httpContextAccessor)
        {
            context = _context;
            mailUtils = _mailUtils;
            webHostEnvironment = _webHostEnvironment;
            httpContextAccessor = _httpContextAccessor; 
        }
        public class PaymentException : Exception
        {
            public PaymentException(string message) : base(message) { }
        }
        public Task CreatePayment(PaymentModel paymentform)
        {
            throw new PaymentException("Cant Create payment ");
        }
        public async Task<PaymentModel> GetPaymentById(int id)
        {
            var payment = await context.PaymentModels
                .Include(c => c.OrderDetails).ThenInclude(c => c.Customers).ThenInclude(s => s.Services)
                .ThenInclude(sb => sb.SubServiceConnections)
                .Include(c => c.OrderDetails).ThenInclude(cccc => cccc.Connections)
                .Include(ac => ac.Accounts)
                .Include(g => g.Guarantees).FirstOrDefaultAsync(c => c.PaymentId == id);
            if (payment != null)
            {
                return payment;
            }
            else
            {
                throw new PaymentException("Cant Get payment with ID : " + id);
            }
        }
        public async Task DeletePayment(int id)
        {
            var pay = await GetPaymentById(id);
            if (pay != null)
            {
                context.PaymentModels.Remove(pay);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new PaymentException("Cant delete payment with ID : " + id);
            }
        }
        public async Task<List<PaymentModel>> GetAllPayment()
        {
            var payment = await context.PaymentModels.Include(ac => ac.Accounts).Include(g => g.Guarantees).Include(c => c.OrderDetails).ThenInclude(css => css.Customers).ToListAsync();
            if (payment != null)
            {
                return payment;
            }
            else
            {
                throw new PaymentException("List payment is empty");
            }
        }
        public async Task UpdatePayment(PaymentModel paymentform, string status)
        {
            if (paymentform != null)
            {
                var payment = await GetPaymentById(paymentform.PaymentId);
                if (payment != null)
                {
                    payment.SendMail = true;
                    payment.UpdatedDate = DateTime.Now;
                    payment.OrderDetails.Connections.UpdatedDate = DateTime.Now;
                    payment.PaymentMode = status;
                    payment.PaymentAmount = payment.PaymentAmount - paymentform.PaymentAmount;
                    payment.Accounts.Status = "true";
                    payment.OrderDetails.Customers.RegistrationStatus = true;
                    payment.OrderDetails.Connections.Status = "true";
                    context.PaymentModels.Update(payment);
                    context.Entry(payment).Property(a => a.AccountRefId).IsModified = false;
                    context.Entry(payment).Property(a => a.GuaranteeRefId).IsModified = false;
                    context.Entry(payment).Property(a => a.GuaranteeRefId).IsModified = false;
                    context.Entry(payment).Property(a => a.OrderDetailRefId).IsModified = false;
                    var result = await context.SaveChangesAsync();
                    if (result > 0 && payment.SendMail == true)
                    {
                        //
                        var email = new MailContextDTO();
                        email.Name = "Nesux Corporation Register";
                        email.To = payment.OrderDetails.Customers.Email;
                        email.Subject = "Hello, " + payment.OrderDetails.Customers.Name;
                        string viewsPath = Path.Combine(webHostEnvironment.ContentRootPath, "Areas/Financial/Views/ThankYou/ThankYou.cshtml");
                        string emailTemplate = await System.IO.File.ReadAllTextAsync(viewsPath);
                        emailTemplate = emailTemplate.Replace("{{CustomerName}}", payment.OrderDetails.Customers.Name);
                        emailTemplate = emailTemplate.Replace("{{CustomerEmail}}", payment.OrderDetails.Customers.Email);
                        emailTemplate = emailTemplate.Replace("{{CustomerPassword}}", payment.OrderDetails.Customers.Password);
                        emailTemplate = emailTemplate.Replace("{{AccountCode}}", payment.Accounts.AccountCode);
                        emailTemplate = emailTemplate.Replace("{{AccountPassword}}", payment.Accounts.Password);
                        email.Body = emailTemplate;
                        await mailUtils.SendMail(email);
                        //



                        var transaction = new TransactionViewModel
                        {
                            Time = DateTime.Now,
                            Description = " Payment: " + paymentform.PaymentId + "Received :"  + payment.OrderDetails.Customers.Services.ServicePrice +  "Done",
                            Type = "PaymentReceived"
                        };
                        var transactionsJson = httpContextAccessor.HttpContext.Session.GetString("Transactions");
                        var transactions = string.IsNullOrEmpty(transactionsJson) ?
                            new List<TransactionViewModel>() :
                            JsonConvert.DeserializeObject<List<TransactionViewModel>>(transactionsJson);
                        transactions.Add(transaction);
                        httpContextAccessor.HttpContext.Session.SetString("Transactions", JsonConvert.SerializeObject(transactions));

                    }
                }
                else
                {
                    throw new PaymentException("payment null");
                }
            }
            else
            {
                throw new PaymentException("Cant payment");
            }
        }
        public async Task<Dictionary<string, decimal>> DashBoard(string sortBy)
        {
            var list = await context.PaymentModels
        .Include(ac => ac.Accounts).ThenInclude(ac => ac.Customer).ThenInclude(cs => cs.Services)
        .Where(c => c.PaymentAmount == 0)
        .ToListAsync();

            if (list != null)
            {
                var servicePriceByPeriod = new Dictionary<string, decimal>();
                DateTime currentDate = DateTime.Now;

                foreach (var payment in list)
                {
                    if (payment != null && payment.PaymentAmount == 0 && payment.UpdatedDate.HasValue && payment.Accounts!=null)
                    {
                        decimal totalServicesPrice = 0;

                        DateTime orderDate = payment.UpdatedDate.Value;
                        string period = "";

                        if (sortBy == "month" && orderDate.Year == currentDate.Year && orderDate.Month == currentDate.Month)
                        {
                            var startDate = new DateTime(orderDate.Year, orderDate.Month, 1);
                            var endDate = new DateTime(orderDate.Year, orderDate.Month, DateTime.DaysInMonth(orderDate.Year, orderDate.Month));
                            period = $"{startDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd}";
                            if (payment.Accounts.Customer != null && payment.Accounts.Customer.Services!=null)
                            {
                                totalServicesPrice += payment.Accounts.Customer.Services.ServicePrice;
                            }
                            else
                            {
                                totalServicesPrice += 0;
                            }
                        }
                        else if (sortBy == "lastmonth" && orderDate.Year == currentDate.Year && orderDate.Month == currentDate.Month - 1)
                        {
                            var startDate = new DateTime(orderDate.Year, orderDate.Month, 1);
                            var endDate = new DateTime(orderDate.Year, orderDate.Month, DateTime.DaysInMonth(orderDate.Year, orderDate.Month));
                            period = $"{startDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd}";
                            /* totalServicesPrice += payment.Accounts.Customer.Services.ServicePrice;*/
                            if (payment.Accounts.Customer != null && payment.Accounts.Customer.Services != null)
                            {
                                totalServicesPrice += payment.Accounts.Customer.Services.ServicePrice;
                            }
                            else
                            {
                                totalServicesPrice += 0;
                            }
                        }
                        else if (sortBy == "year" && orderDate.Year == currentDate.Year)
                        {
                            var startDate = new DateTime(orderDate.Year, 1, 1);
                            var endDate = new DateTime(orderDate.Year, 12, 31);
                            period = $"{startDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd}";
                            /*totalServicesPrice += payment.Accounts.Customer.Services.ServicePrice;*/
                            if (payment.Accounts.Customer != null && payment.Accounts.Customer.Services != null)
                            {
                                totalServicesPrice += payment.Accounts.Customer.Services.ServicePrice;
                            }
                            else
                            {
                                totalServicesPrice += 0;
                            }
                        }
                        else if (sortBy == "lastyear" && orderDate.Year == currentDate.Year - 1)
                        {
                            var startDate = new DateTime(orderDate.Year - 1, 1, 1);
                            var endDate = new DateTime(orderDate.Year - 1, 12, 31);
                            period = $"{startDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd}";
                            /* totalServicesPrice += payment.Accounts.Customer.Services.ServicePrice;*/
                            if (payment.Accounts.Customer != null && payment.Accounts.Customer.Services != null)
                            {
                                totalServicesPrice += payment.Accounts.Customer.Services.ServicePrice;
                            }
                            else
                            {
                                totalServicesPrice += 0;
                            }
                        }

                        if (!string.IsNullOrEmpty(period))
                        {
                            if (servicePriceByPeriod.ContainsKey(period))
                                servicePriceByPeriod[period] += totalServicesPrice;
                            else
                                servicePriceByPeriod[period] = totalServicesPrice;
                        }
                    }
                }

                return servicePriceByPeriod;
            }
            else
            {
                throw new PaymentException("DashBoard is empty");
            }
        }
        public async Task<List<MonthData>> GetMonthlyData()
        {
            var monthlyData = await context.PaymentModels
            .Include(pm => pm.Accounts)
                .ThenInclude(ac => ac.Customer)
                    .ThenInclude(cu => cu.Services)
            .Where(pm => pm.PaymentAmount == 0)
            .ToListAsync();
            var monthlyServicePrices = new List<MonthData>();

            foreach (var paymentModel in monthlyData)
            {
                DateTime month = paymentModel.UpdatedDate.GetValueOrDefault();
                var existingData = monthlyServicePrices.FirstOrDefault(md => md.Month == month);
                decimal servicePrice = paymentModel.Accounts.Customer.Services.ServicePrice;
                if (existingData != null)
                {
                    existingData.Earnings += paymentModel.Accounts.Customer.Services.ServicePrice;
                }
                else
                {
                    var newData = new MonthData
                    {
                        Month = month,
                        Earnings = paymentModel.Accounts.Customer.Services.ServicePrice
                    };
                    monthlyServicePrices.Add(newData);
                }
            }
            return monthlyServicePrices;
        }

        public async Task<PaymentModel> GetPaymentByCusId(int id)
        {
           var Pay = await context.PaymentModels.Include(o=>o.OrderDetails).ThenInclude(c=>c.Customers).FirstOrDefaultAsync(c=>c.OrderDetails.CustomerRefId == id);
            if (Pay != null)
            { 
                return Pay;
            }
            else
            {
                throw new PaymentException("Cant get Payment by this Cus");
            }

        }
    }
}
