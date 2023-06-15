using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NexusApp.Areas.Customer.Models;
using NexusApp.Areas.ServiceConnection.Models;
using NexusApp.Areas.Survey.Models;
using NexusApp.Data;
using NexusApp.MailForm;
using NexusApp.ModelDTOs;
using static NexusApp.ModelDTOs.DashboardViewModel;

namespace NexusApp.Areas.Financial.Reposetory.Customer
{
    public class CustomerServiceImp : ICustomerRepository
    {
        private readonly ApplicationDbContext context;
        public readonly MailUtils mailUtils;
        private readonly EmailSender emailSender;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration configuration;
        public CustomerServiceImp(ApplicationDbContext _context,
            MailUtils _mailUtils, EmailSender _emailSender, 
            IWebHostEnvironment _webHostEnvironment, IHttpContextAccessor _httpContextAccessor, IConfiguration _configuration)
        {
            context = _context;
            mailUtils = _mailUtils;
            emailSender = _emailSender;
            webHostEnvironment = _webHostEnvironment;
            httpContextAccessor = _httpContextAccessor;
            configuration = _configuration;
        }
        public class CustomerException : Exception
        {
            public CustomerException(string message) : base(message){}
        }
        public async Task AddCustomer(CustomerModel customer)
        {
            if(customer != null)
            {
                await context.customerModels.AddAsync(customer);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new CustomerException("Can not Add Customer ");
            }
        }
        public async Task<List<CustomerModel>> FindCustomerByScon(int id)
        {
            var cus = await (from c in context.customerModels
                             join s in context.serviceModels on c.ServiceRefId equals s.ServiceId
                             join sb in context.subServiceConnectionModels on s.SubServiceConnectionRefId equals sb.SubServiceConnectionId
                             join sc in context.serviceConnectionModels on sb.ServiceConnectionRefId equals sc.ServiceConnectionId
                             where sc.ServiceConnectionId == id
                             select new CustomerModel
                             {
                                 Name = c.Name,Phone = c.Phone,Email = c.Email,City = c.City,
                                 Services = new ServiceModel
                                 {
                                     Name = s.Name,
                                     SubServiceConnections = new SubServiceConnectionModel
                                     {
                                         Name = sb.Name,
                                         ServiceConnections = new ServiceConnectionModel
                                         {
                                             Name = sc.Name,
                                         }
                                     }
                                 }
                             }).ToListAsync();

            if (cus != null)
            {
                return cus;
            }
            else
            {
                throw new CustomerException("Can not find any Customer with by this Scon");
            }
        }
        public async Task DeleteCustomer(int id)
        {
            var cus = await context.customerModels.FindAsync();
            if (cus != null )
            {
                context.customerModels.Remove(cus);
                context.SaveChanges();
            }
            else
            {
                throw new CustomerException("Can not Delete Customer with this ID");
            }
        }
        public async Task<List<CustomerModel>> GetAllCustomer()
        {
            var cus =  await context.customerModels.Include(s => s.Services).ThenInclude(sb => sb.SubServiceConnections).
                                                    ThenInclude(sc => sc.ServiceConnections).Include(ses=>ses.Surveys).ToListAsync();
            if (cus != null)
            {
               return cus;
            }else
            {
                throw new CustomerException("Can not get List Customer");
            }
        }
        public async Task<CustomerModel> GetCustomerByID(int id)
        {
            var cus = await context.customerModels.FindAsync(id);
            if (cus != null)
            {
                return cus;
            }
            else
            {
                throw new CustomerException("Can not get Customer by this ID");
            }
        }
        public async Task UpdateCustomer(CustomerModel cusmodelform, int EmpID)
        {
            var cusmodels = await context.customerModels.FindAsync(cusmodelform.CustomerId);
            if (cusmodels != null)
            {
                cusmodels.RegistrationStatus = cusmodels.RegistrationStatus;
                cusmodels.UpdatedDate = cusmodelform.UpdatedDate;
                cusmodels.Name = cusmodelform.Name;
                cusmodels.BirthDay = cusmodelform.BirthDay;
                cusmodels.Street = cusmodelform.Street;
                cusmodels.Ward = cusmodelform.Ward;
                cusmodels.District = cusmodelform.District;
                cusmodels.City = cusmodelform.City;
                cusmodels.Email = cusmodelform.Email;
                cusmodels.Phone = cusmodelform.Phone;
                cusmodels.ServiceRefId = cusmodelform.ServiceRefId;
                cusmodels.CreatedDate = DateTime.Now;
                cusmodels.UpdatedDate = DateTime.Now;
                context.customerModels.Update(cusmodels);
                var result = await context.SaveChangesAsync();
                if (result >0  && cusmodels.CustomerId !=null)
                {
                    var extingSur =  context.surveyModels.FirstOrDefault(s=>s.CustomerRefId == cusmodels.CustomerId);
                    if (extingSur != null)
                    {
                        var model = new SurveyModel();
                        model.CustomerRefId = cusmodels.CustomerId;
                        model.ServeyName = "";
                        model.Status = "Pending";
                        model.SurveyDate = DateTime.Now;
                        model.CreatedDate = DateTime.Now;
                        model.EmployeeRefId = EmpID;
                        context.surveyModels.Update(extingSur);
                        await context.SaveChangesAsync();

                        var transaction = new TransactionViewModel
                        {
                            Time = DateTime.Now,
                            Description = "Customer : "+ cusmodelform.Name + "Make new Survey",
                            Type = "NewArrival"
                        };
                        var transactionsJson = httpContextAccessor.HttpContext.Session.GetString("Transactions");
                        var transactions = string.IsNullOrEmpty(transactionsJson) ?
                            new List<TransactionViewModel>() :
                            JsonConvert.DeserializeObject<List<TransactionViewModel>>(transactionsJson);
                        transactions.Add(transaction);
                        httpContextAccessor.HttpContext.Session.SetString("Transactions", JsonConvert.SerializeObject(transactions));

                    }
                    else
                    {
                        var model = new SurveyModel();
                        model.CustomerRefId = cusmodels.CustomerId;
                        model.ServeyName = "";
                        model.Status = "Pending";
                        model.SurveyDate = DateTime.Now;
                        model.CreatedDate = DateTime.Now;
                        model.EmployeeRefId = EmpID;
                        context.surveyModels.Add(model);
                        await context.SaveChangesAsync();
                        var transaction = new TransactionViewModel
                        {
                            Time = DateTime.Now,
                            Description = "Customer : " + cusmodelform.Name + "Update Survey",
                            Type = "NewArrival"
                        };
                        var transactionsJson = httpContextAccessor.HttpContext.Session.GetString("Transactions");
                        var transactions = string.IsNullOrEmpty(transactionsJson) ?
                            new List<TransactionViewModel>() :
                            JsonConvert.DeserializeObject<List<TransactionViewModel>>(transactionsJson);
                        transactions.Add(transaction);
                        httpContextAccessor.HttpContext.Session.SetString("Transactions", JsonConvert.SerializeObject(transactions));
                    }

                }
            }
            else
            {
                throw new CustomerException("Can not Update Customer");
            }
        }

        public async Task AddRegiserCustomer(dynamic customer)
        {
            if(customer !=null)
            {
             CustomerModel customerModel = await ConvertToCustomerModel(customer);
                if(customerModel != null)
                 {
                    var exticust = await context.customerModels.FirstOrDefaultAsync(x => x.Email == customerModel.Email);
                    if(exticust == null)
                    {
                        Random random = new Random();
                        string randomNumber = random.Next(10000, 99999).ToString();
                        var model = new CustomerModel();
                        model.Name = customer.Name;
                        model.Email = customer.Email;
                        model.Phone = customer.Phone;
                        model.Password = randomNumber;
                        model.ServiceRefId = customer.ServiceRefId;
                        context.customerModels.Add(model);
                        var result = await context.SaveChangesAsync();
                        if (result > 0)
                        {
                            var email = new MailContextDTO();
                            email.Name = "Nesux Corporation Register";
                            email.To = customer.Email;
                            email.Subject = "Hello, " + customer.Name ;
                            var serviceName = await context.serviceModels
                                .Include(sb=>sb.SubServiceConnections).ThenInclude(sc=>sc.ServiceConnections).FirstOrDefaultAsync(c=>c.ServiceId == customerModel.ServiceRefId);                                                       
                            string viewsPath = Path.Combine(webHostEnvironment.ContentRootPath, "Areas/Financial/Views/ThankYou/RegisterNew.cshtml");
                            string emailTemplate = await System.IO.File.ReadAllTextAsync(viewsPath);
                            emailTemplate = emailTemplate.Replace("{{CustomerName}}", customer.Name);
                            emailTemplate = emailTemplate.Replace("{{CustomerEmail}}", customer.Email);
                            emailTemplate = emailTemplate.Replace("{{CustomerPhone}}", customer.Phone);
                            emailTemplate = emailTemplate.Replace("{{Service}}", serviceName.Name);
                            emailTemplate = emailTemplate.Replace("{{SubService}}", serviceName.SubServiceConnections.Name);
                            emailTemplate = emailTemplate.Replace("{{SconService}}", serviceName.SubServiceConnections.ServiceConnections.Name);
                            email.Body = emailTemplate;
                            await mailUtils.SendMail(email);
                            var transaction = new TransactionViewModel
                            {
                                Time = DateTime.Now,
                                Description = "Customer: "+ customer.Name + "Regiser Service : "+ serviceName.Name,
                                Type = "NewSale"
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
                        Random random = new Random();
                        string randomNumber = random.Next(10000, 99999).ToString();
                        var model = new CustomerModel();
                        model.Name = customer.Name;
                        model.Email = customer.Email;
                        model.Phone = customer.Phone;
                        model.Password = randomNumber;
                        model.ServiceRefId = customer.ServiceRefId;

                        context.customerModels.Update(exticust);
                        var result = await context.SaveChangesAsync();
                        if (result > 0)
                        {
                            var email = new MailContextDTO();
                            email.Name = "Nesux Corporation Register";
                            email.To = customer.Email;
                            email.Subject = "Hello, " + customer.Name;
                            var serviceName = await context.serviceModels
                                .Include(sb => sb.SubServiceConnections).ThenInclude(sc => sc.ServiceConnections).FirstOrDefaultAsync(c => c.ServiceId == customerModel.ServiceRefId);
                            string viewsPath = Path.Combine(webHostEnvironment.ContentRootPath, "Areas/Financial/Views/ThankYou/RegisterUpdate.cshtml");
                            string emailTemplate = await System.IO.File.ReadAllTextAsync(viewsPath);
                            emailTemplate = emailTemplate.Replace("{{CustomerName}}", customer.Name);
                            emailTemplate = emailTemplate.Replace("{{CustomerEmail}}", customer.Email);
                            emailTemplate = emailTemplate.Replace("{{CustomerPhone}}", customer.Phone);
                            emailTemplate = emailTemplate.Replace("{{Service}}", serviceName.Name);
                            emailTemplate = emailTemplate.Replace("{{SubService}}", serviceName.SubServiceConnections.Name);
                            emailTemplate = emailTemplate.Replace("{{SconService}}", serviceName.SubServiceConnections.ServiceConnections.Name);
                            email.Body = emailTemplate;
                            await mailUtils.SendMail(email);
                            var transaction = new TransactionViewModel
                            {
                                Time = DateTime.Now,
                                Description = "Customer : "+ customer.Name + " Update Info",
                                Type = "NewSale"
                            };
                            var transactionsJson = httpContextAccessor.HttpContext.Session.GetString("Transactions");
                            var transactions = string.IsNullOrEmpty(transactionsJson) ?
                                new List<TransactionViewModel>() :
                                JsonConvert.DeserializeObject<List<TransactionViewModel>>(transactionsJson);
                            transactions.Add(transaction);
                            httpContextAccessor.HttpContext.Session.SetString("Transactions", JsonConvert.SerializeObject(transactions));

                        }
                    }
                }
            }
            else
            {
                throw new CustomerException("Can not Register Customer");
            }
        }
        private Task<CustomerModel> ConvertToCustomerModel(dynamic customer)
        {
            if(customer != null)
            {
                CustomerModel customerModel = new CustomerModel(); 
                customerModel.Name = customer.Name;
                customerModel.Email = customer.Email;
                customerModel.Phone = customer.Phone;
                customerModel.ServiceRefId = customer.ServiceRefId;
                return Task.FromResult(customerModel);
            }
            else
            {
                throw new CustomerException("Can not Converrt Customer");
            }
           
        }
    }
}
