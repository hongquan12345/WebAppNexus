using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using NexusApp.Areas.Customer.Models;
using NexusApp.Areas.Financial.Models;
using NexusApp.Areas.Financial.Models.ModelsViews;
using NexusApp.Areas.Financial.Reposetory.Account;
using NexusApp.Areas.Financial.Reposetory.Connect;
using NexusApp.Areas.Financial.Reposetory.Customer;
using NexusApp.Areas.Financial.Reposetory.Guarantee;
using NexusApp.Areas.Financial.Reposetory.Install;
using NexusApp.Areas.Financial.Reposetory.Order;
using NexusApp.Areas.Financial.Reposetory.Payment;
using NexusApp.Areas.Financial.Reposetory.ServiceConnection;
using NexusApp.Areas.Financial.Reposetory.Survey;
using NexusApp.Areas.Survey.Models;
using NexusApp.Data;
using System.Data;
using X.PagedList;
using PayPal.Api;
using static NexusApp.Areas.Financial.Reposetory.Customer.CustomerServiceImp;
using static NexusApp.Areas.Financial.Reposetory.Guarantee.GuaranteeImplement;
using static NexusApp.Areas.Financial.Reposetory.Order.OrderImplement;
using static NexusApp.Areas.Financial.Reposetory.Survey.Surveyimplement;
using NexusApp.Constants;
using Azure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using NexusApp.Areas.Employee.Repository;

namespace NexusApp.Areas.Financial.Controllers
{
   /* [CustomAuthorization(Role = "Admin")]*/
    [Area("Financial")]
    public class FinancialController : Controller
    {
        private IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext context;
        private readonly ICustomerRepository customerRepository;
        private readonly IServiceConnectionRepository serviceCon;
        private readonly ISurveyReposetory surveyRepo;
        private readonly InstallReposetory installReposetory;
        private readonly IGuaranteeReposetory guaranteeReposetory;
        private readonly IOrderReposetory orderReposetory;
        private readonly IPaymentReposetory paymentReposetory;
        private readonly IAccountReposetory accountReposetory;
        private readonly IConnectionReposetory connectionReposetory;
        private readonly IEmployeeRepository employeeRepository;

        public FinancialController(ApplicationDbContext _context,ICustomerRepository _customerRepository,IServiceConnectionRepository _serviceCon,
            ISurveyReposetory _Survey, InstallReposetory _installReposetory, IGuaranteeReposetory _guaranteeReposetory,
            IOrderReposetory _orderReposetory, IPaymentReposetory _paymentReposetory,IAccountReposetory _accountReposetory,
            IConnectionReposetory _connectionReposetory,IHttpContextAccessor httpcontext, IEmployeeRepository _employeeRepository)
            {
                context = _context;
                customerRepository = _customerRepository;
                serviceCon = _serviceCon;
                surveyRepo = _Survey;
                installReposetory   = _installReposetory;
                guaranteeReposetory = _guaranteeReposetory;
                orderReposetory = _orderReposetory;
                paymentReposetory = _paymentReposetory;
                accountReposetory = _accountReposetory;
                connectionReposetory = _connectionReposetory;
                httpContextAccessor = httpcontext;
                employeeRepository = _employeeRepository;
            }
        //----------------CustomerForm--------------------- //
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> CustomerList(int searchConID, string searchName, int page = 1)
        {
            var cus = await customerRepository.GetAllCustomer();
            ViewBag.ListCon = await serviceCon.GetAllServiceConect();
            ViewBag.ListEmp = await employeeRepository.GetEmployeewithrole("Technical");

            if (cus != null)
            {
                if (searchConID != 0)
                {
                    cus = await customerRepository.FindCustomerByScon(searchConID);
                }
                if (!string.IsNullOrEmpty(searchName))
                {
                    cus = cus.Where(c => c.Name.Contains(searchName)).ToList();
                }
                page = page < 1 ? 1 : page;
                int pageSize = 3;
                IPagedList<CustomerModel> pagedCus = cus.ToPagedList(page, pageSize);
                return View(pagedCus);
            }
            return View(cus);
        }
        [CustomAuthorization("Admin", "Accountant")]
        [HttpPost]
        public async Task<IActionResult> UpdateFullCustomer(CustomerModel cus,int EmpID)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await customerRepository.UpdateCustomer(cus, EmpID);
                    return RedirectToAction("CustomerList");
                }
                catch (CustomerException ex)
                {
                    TempData["Error"] = ex.Message;
                    return RedirectToAction("CustomerList");
                }
            }
            else
            {
                TempData["Error"] = "Data Invalid";
                return RedirectToAction("CustomerList");
            }
        }
        //----------------SurveyForm--------------------- //
        [CustomAuthorization("Admin", "Manager","Technical")]
        public async Task<IActionResult> ServeyList(string surveyName,string customerName, int page = 1)
        {
            var suy = await surveyRepo.GetAllSurvey();
            if (suy != null)
            {
                if (!string.IsNullOrEmpty(surveyName))
                {
                    suy = suy.Where(c => c.ServeyName.Contains(surveyName)).ToList();
                }
                if (!string.IsNullOrEmpty(customerName))
                {
                    suy = suy.Where(c => c.Customer?.Name!=null && c.Customer.Name.Contains(customerName)).ToList();
                }
                page = page < 1 ? 1 : page;
                int pageSize = 3;
                IPagedList<SurveyModel> pagedCus = suy.ToPagedList(page, pageSize);
                return View(pagedCus);
            }
            return View(suy);
        }
        [HttpPost]
        [CustomAuthorization("Admin", "Technical")]
        public async Task<IActionResult> UpdateSurvey(SurveyModel survey)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await surveyRepo.UpdateSurvey(survey);
                    return RedirectToAction("ServeyList");
                }
                catch (SurveyException ex)
                {
                    TempData["Error"] = ex.Message;
                    return RedirectToAction("ServeyList");
                }
            }
            return RedirectToAction("ServeyList");
        }
        //----------------GuaranteeForm--------------------- //
        [HttpGet]
        [CustomAuthorization("Admin", "Manager", "Technical")]
        public async Task<IActionResult>GuaranteList(string surveyName, string customerName,string employeeName, int page = 1)
        {
            var gua = await guaranteeReposetory.GetAllGuarantee();
            if(gua != null)
            {
                if (!string.IsNullOrEmpty(surveyName))
                {
                    gua = gua.Where(c => c.surveyModel.ServeyName.Contains(surveyName)).ToList();
                }
                if (!string.IsNullOrEmpty(customerName))
                {
                    gua = gua.Where(c => c.surveyModel.Customer.Name.Contains(customerName)).ToList();
                }
                if(!string.IsNullOrEmpty(employeeName))
                {
                    gua = gua .Where(e=>e.surveyModel.Employee.Name.Contains(employeeName)).ToList();
                }
                page = page < 1 ? 1 : page;
                int pageSize = 3;
                IPagedList<GuaranteeModel> pagedCus = gua.ToPagedList(page, pageSize);
                return View(pagedCus);
            }
            return View(gua);
        }
        [HttpPost]
        [CustomAuthorization("Admin", "Technical")]
        public async Task<IActionResult> UpdateGuarantee(GuaranteeModel guarantee)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    await guaranteeReposetory.UpdateGuarantee(guarantee);
                    return RedirectToAction("GuaranteList");
                }
                catch(GuaranteeExceptio ex)
                {
                    TempData["Error"] = ex.Message;
                    return RedirectToAction("GuaranteList");
                }
            }
            return RedirectToAction("GuaranteList");
        }
        //----------------GuaranteeForm--------------------- //
        //----------------InstalltForm--------------------- //
        [HttpGet]
        [CustomAuthorization("Admin", "Manager", "Technical")]
        public async Task<IActionResult> InstallList()
        {
            var ins = await installReposetory.GetAllInstall();
            var gua = await guaranteeReposetory.GetAllGuarantee();
            var data = new InstallGuaranteeViewModel
            {
                InstallModelView = ins,
                GuaModelView = gua,
            };
            if (data != null)
            {
                return View(data);
            }
            return View();
        }
        [HttpPost]
        [CustomAuthorization("Admin", "Technical")]
        public async Task<IActionResult>UpdateIns(InstallModel install)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await installReposetory.UpdateInstall(install);
                    return RedirectToAction("InstallList");
                }
                catch (SurveyException ex)
                {
                    TempData["Error"] = ex.Message;
                    return RedirectToAction("InstallList");
                }            
            }
            return RedirectToAction("InstallList");
        }
        //----------------OrderForm--------------------- //
        [HttpGet]
        [CustomAuthorization("Admin", "Manager", "Accountant")]
        public async Task <IActionResult> OrderList(int page = 1)
        {
            var order = await orderReposetory.GetAllOrderDetail();
            if (order != null)
            {
                page = page < 1 ? 1 : page;
                int pageSize = 5;
                IPagedList<OrderDetailModel> orderlist = order.ToPagedList(page, pageSize);
                return View(orderlist);
            }
            return View();
        }
        [HttpPost]
        [CustomAuthorization("Admin", "Accountant")]
        public async Task<IActionResult>UpdateOrderDetail(OrderDetailModel order)
        { 
            if(ModelState.IsValid)
            {
                try
                {
                    await orderReposetory.UpdateOrderDetail(order);
                    return RedirectToAction("OrderList");

                }
                catch(OrderDetailException ex)
                {
                    TempData["Error"] = ex.Message;
                    return RedirectToAction("OrderList");
                }
            }
            else
            {
                var errors = ModelState.Values
            .Where(v => v.ValidationState == ModelValidationState.Invalid)
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

                ViewBag.Errors = errors;
                return View("OrderList");
            }
        }
        //----------------PaymentForm--------------------- //
        [HttpGet]
        [CustomAuthorization("Admin","Manager", "Accountant")]
        public async Task<IActionResult>PaymentList(int page = 1)
        {
            var paylist = await paymentReposetory.GetAllPayment();
            if (paylist != null)
            {
                page = page < 1 ? 1 : page;
                int pageSize = 5;
                IPagedList<PaymentModel> pagedP = paylist.ToPagedList(page, pageSize);
                return View(pagedP);            
            }    
            return View();
        }
        [HttpGet]

        [CustomAuthorization("Admin", "Accountant")]
        public async Task<IActionResult> PaymentUpdate(int paymentId, string paymentMode)
        {
            if(paymentId == 0)
            {
                var payID = httpContextAccessor.HttpContext.Session.GetInt32("PaymentId");
                var Model2 = await paymentReposetory.GetPaymentById(payID.Value);
                if (Model2 != null)
                {
                    paymentMode = "Online";
                    Model2.PaymentMode = paymentMode;
                    ViewBag.PaymentMode = paymentMode;
                    return View(Model2);
                }
            }
            else
            {
                var Model = await paymentReposetory.GetPaymentById(paymentId);
                if (Model != null)
                {
                    ViewBag.PaymentMode = paymentMode;
                    return View(Model);
                }
            }
              return View();
        }
        [HttpPost]
        [CustomAuthorization("Admin", "Accountant")]
        public async Task<IActionResult>PaymentOffline(PaymentModel paymentModel)
        {
            ModelState.Remove("Accounts");
            ModelState.Remove("Guarantees");
            if(ModelState.IsValid)
            {
                var ModelPay = await paymentReposetory.GetPaymentById(paymentModel.PaymentId);
                await paymentReposetory.UpdatePayment(ModelPay,paymentModel.PaymentMode);
                return RedirectToAction("PaymentSuccess", "Payment");
            }
            else
            {
                return RedirectToAction("PaymentFailed", "Payment");
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> PaymentPaypal(PaymentModel Model, string Cancel, string PayerID, string guid, string blogId)
        {
            Model.PaymentMode = "Online";
            var ClientID = PayPalService.Client_ID;
            var ClientSecret = PayPalService.Secret_key;
            var mode = PayPalService.mode;
            APIContext apiContext = PaypaylConfiguration.GetAPIContext(ClientID, ClientSecret, mode);
            try
            {
                if (Cancel != "true")
                {
                    string payerId = PayerID;
                    if (string.IsNullOrEmpty(payerId))
                    {
                        var ModelPay = await paymentReposetory.GetPaymentById(Model.PaymentId);
                        string baseURI = this.Request.Scheme + "://" + this.Request.Host + $"/Financial/Financial/PaymentPaypal?";
                        var guildd = Convert.ToString((new Random()).Next(10000000));
                        guid = guildd;
                        var createPayment = await this.Createpayment(apiContext, baseURI + "guid" + guid, blogId, ModelPay);
                        var links = createPayment.links.GetEnumerator();
                        string paypalRedirectUrl = null;
                        while (links.MoveNext())
                        {
                            Links lnk = links.Current;
                            if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                            {
                                paypalRedirectUrl = lnk.href;
                            }
                        }
                        httpContextAccessor.HttpContext.Session.SetString("payment", createPayment.id);
                        httpContextAccessor.HttpContext.Session.SetString("paymentID", Model.PaymentId.ToString());
                        return Redirect(paypalRedirectUrl);
                    }
                    else
                    {
                        var paymentId = httpContextAccessor.HttpContext.Session.GetString("payment");
                        var excutedPayment = ExcutePayment(apiContext, payerId, paymentId.ToString());
                        if (excutedPayment.state.ToLower() == "approved")
                        {
                            var blogIds = excutedPayment.transactions[0].item_list.items[0].sku;
                            var paymentuserId = httpContextAccessor.HttpContext.Session.GetString("paymentID");
                            int orderNumber = int.Parse(paymentuserId);
                            var ModelPay = await paymentReposetory.GetPaymentById(orderNumber);
                            ModelState.Remove("Accounts");
                            ModelState.Remove("Guarantees");
                            await paymentReposetory.UpdatePayment(ModelPay, "Paypal");
                            return RedirectToAction("PaymentSuccess", "User", new { area = "" });
                        }

                        return RedirectToAction("PaymentFailed", "User",new { area = "" });

                    }
                }
                else
                {
                    return RedirectToAction("PaymentFailed", "User", new { area = "" });

                }
            }
            catch (Exception)
            {
                var errors = ModelState.Values
                .Where(v => v.ValidationState == ModelValidationState.Invalid)
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
                ViewBag.Errors = errors;
                return View("PaymentList");
            }
        }
        private PayPal.Api.Payment payment;
        private Payment ExcutePayment(APIContext apiContext, string payerId, string paymentID)
        {
            var paymentExcution = new PaymentExecution()
            {
                payer_id = payerId,
            };
            this.payment = new Payment()
            {
                id = paymentID,
            };
            return this.payment.Execute(apiContext, paymentExcution);
        }
        [CustomAuthorization("Admin", "Accountant")]
        private async Task<Payment> Createpayment(APIContext apiContext, string redirectUrl, string blogId, PaymentModel Model)
        {
            var ModelPay = await paymentReposetory.GetPaymentById(Model.PaymentId);
            var random = new Random();
            var randomSuffix = random.Next(1000, 9999);
            var paypalOrderId = $"{ModelPay.OrderDetails.OrderSeri}-{randomSuffix}";
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            var total = Model.PaymentAmount.ToString();
            itemList.items.Add(new Item()
            {
                name = ModelPay.OrderDetails.Connections.ConnectionName.ToString(),
                currency = "USD",
                price = Model.PaymentAmount.ToString(),
                quantity = 1.ToString(),
                sku = $"{ModelPay.OrderDetails.OrderSeri}-{randomSuffix}",
                tax = "0"
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            var redirectUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            var details = new Details
            {
                tax = "0",
                shipping = "0",
                subtotal = total.ToString()
            };
            var amount = new Amount()
            {
                total = total.ToString(),
                currency = "USD",
            };
            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Transaction descripttion",
                invoice_number = paypalOrderId.ToString(),
                amount = amount,
                item_list = itemList,
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirectUrls
            };
            return payment.Create(apiContext);
        }
        //----------------ConnectForm--------------------- //
        [CustomAuthorization("Admin", "Manager", "Accountant")]
        public async Task<IActionResult> ConnectList(int page=1)
        {
            var model = await connectionReposetory.GetAllConnect();
            List<int?> daysElapsedList = new List<int?>();
            if (model != null)
            {
                foreach (var item in model)
                {
                    var time = item.Services?.Duration;
                    DateTime? updateDate = item.UpdatedDate;
                    if (updateDate != null)
                    {
                        DateTime? endDate = updateDate.HasValue ? updateDate.Value.AddMonths(time ?? 0) : null;
                        TimeSpan? timeElapsed = endDate - DateTime.Now;
                        int? daysElapsed = timeElapsed?.Days;
                        await connectionReposetory.UpdateConnection(item, daysElapsed);                   
                        daysElapsedList.Add(daysElapsed);
                    }
                    else
                    {
                        TimeSpan? timeElapsed = DateTime.Now - DateTime.Now;
                        int? daysElapsed = timeElapsed?.Days;
                        await connectionReposetory.UpdateConnection(item, daysElapsed);                 
                        daysElapsedList.Add(daysElapsed);
                    }
                }
                ViewBag.TimeElapsedList = daysElapsedList;
                page = page < 1 ? 1 : page;
                int pageSize = 5;
                IPagedList<ConnectionModel> pagedC = model.ToPagedList(page, pageSize);
                return View(pagedC);
            }
            return View(model);
        }
        //----------------AccountForm--------------------- //
        [CustomAuthorization("Admin", "Manager", "Accountant")]
        public async Task<IActionResult> AccountList(int page = 1)
        {
            var model = await accountReposetory.GetAllAcount();
            if(model != null)
            {
                page = page < 1 ? 1 : page;
                int pageSize = 5;
                IPagedList<AccountModel> pagedA = model.ToPagedList(page, pageSize);
                return View(pagedA);
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        [CustomAuthorization("Admin", "Accountant")]
        public async Task<IActionResult>DeleteAccount(int id)
        {
            await accountReposetory.DeleteAccount(id);
            return RedirectToAction("AccountList");  
        }
    }
}
