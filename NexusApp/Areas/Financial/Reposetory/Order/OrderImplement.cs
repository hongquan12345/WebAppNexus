using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NexusApp.Areas.Customer.Models;
using NexusApp.Areas.Financial.Models;
using NexusApp.Areas.Survey.Models;
using NexusApp.Data;
using static NexusApp.ModelDTOs.DashboardViewModel;

namespace NexusApp.Areas.Financial.Reposetory.Order
{
    public class OrderImplement : IOrderReposetory
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public OrderImplement(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor)
        {
            context = _context;
            httpContextAccessor = _httpContextAccessor;
        }
        public class OrderDetailException : Exception
        {
            public OrderDetailException(string message) : base(message) { }
        }
        public async Task<List<OrderDetailModel>> GetAllOrderDetail()
        {
            var order = await context.orderDetailModels.Include(o => o.Payments).Include(c => c.Employees).Include(cc => cc.Connections)
                .Include(cs => cs.Customers).ThenInclude(sc => sc.Surveys).ThenInclude(sscs => sscs.Installs)
                .Include(cs => cs.Customers).ThenInclude(sc => sc.Surveys).ThenInclude(guar => guar.Guarantees)
                .ToListAsync();
            if (order != null)
            {
                return order;
            }
            else
            {
                throw new OrderDetailException("Can't Get List order");
            }
        }
        public Task AddOrderDetail(OrderDetailModel orderDetail)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOrderDetail(int id)
        {
            throw new NotImplementedException();
        }

        
        public async Task<OrderDetailModel> GetOrderDetailByID(int id)
        {
            var orderbyID = await context.orderDetailModels.Include(p => p.Payments).ThenInclude(ac => ac.Accounts)
                .Include(c => c.Connections).Include(easd => easd.Equipments)
                 .Include(cs => cs.Customers).ThenInclude(scs => scs.Surveys).ThenInclude(aaa => aaa.Guarantees)
                 .Include(cs => cs.Customers).ThenInclude(s => s.Services)
                 .SingleOrDefaultAsync(c => c.OrderDetailId == id);
            if(orderbyID != null)
            {
                return orderbyID;
            }
            else
            {
                throw new OrderDetailException("Can't Get order by Id :" + id);
            }

        }

        public async Task UpdateOrderDetail(OrderDetailModel OrderDetail)
        {
            var Order = await GetOrderDetailByID(OrderDetail.OrderDetailId);

            if (Order != null && Order.Customers != null)
            {
                Order.UpdatedDate = DateTime.Now;
                context.orderDetailModels.Update(Order);
                await context.SaveChangesAsync();
                var transaction = new TransactionViewModel
                {
                    Time = DateTime.Now,
                    Description = " Order: " + OrderDetail.OrderSeri + " Have Update",
                    Type = "PaymentMade"
                };
                var transactionsJson = httpContextAccessor.HttpContext.Session.GetString("Transactions");
                var transactions = string.IsNullOrEmpty(transactionsJson) ?
                    new List<TransactionViewModel>() :
                    JsonConvert.DeserializeObject<List<TransactionViewModel>>(transactionsJson);
                transactions.Add(transaction);
                httpContextAccessor.HttpContext.Session.SetString("Transactions", JsonConvert.SerializeObject(transactions));
                var extingAcount = await context.accountModels.FirstOrDefaultAsync(s => s.CustomerRefId == Order.CustomerRefId);
                if (extingAcount == null && Order.Connections!=null)
                {
                   
                    Random randomac = new Random();
                    string randomacs = new string(Enumerable.Repeat("0123456789", 3)
                                                .Select(s => s[randomac.Next(s.Length)]).ToArray());
                    var account = new AccountModel();
                    account.Status = "Pending";
                    account.AccountCode = Order.Connections.ConnectionType[0].ToString() + randomacs + Order.OrderSeri.ToString().Substring(0,12);
                    account.CustomerRefId = Order.CustomerRefId;
                    account.CreatedDate = DateTime.Now;
                    Random random = new Random();
                    string randomPassword = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", 12)
                                                .Select(s => s[random.Next(s.Length)]).ToArray());
                    account.Password = randomPassword;
                    context.accountModels.Add(account);
                    var result = await context.SaveChangesAsync();

                     transaction = new TransactionViewModel
                    {
                        Time = DateTime.Now,
                        Description = " Account: " + Order.Connections.ConnectionType[0].ToString() + randomacs + Order.OrderSeri.ToString().Substring(0, 12) + " Have Created, Status : Pending",
                        Type = "NewArrival"
                     };
                    transactionsJson = httpContextAccessor.HttpContext.Session.GetString("Transactions");
                    transactions = string.IsNullOrEmpty(transactionsJson) ?
                        new List<TransactionViewModel>() :
                        JsonConvert.DeserializeObject<List<TransactionViewModel>>(transactionsJson);
                    transactions.Add(transaction);

                    httpContextAccessor.HttpContext.Session.SetString("Transactions", JsonConvert.SerializeObject(transactions));

                    if (result > 0 && Order.Customers != null && Order.Customers.Surveys != null && account.AccountId >0)
                    {
                        var payment = new PaymentModel();
                        payment.SendMail = false;
                        payment.PaymentMode = "Pending";
                        payment.CreatedDate = DateTime.Now;
                        payment.OrderDetailRefId = Order.OrderDetailId;
                        var ServicePrice = 0;
                        foreach (var Surveys in Order.Customers.Surveys)
                        {
                            payment.GuaranteeRefId = Surveys.Guarantees.GuaranteeId;
                            var amount = Surveys.Guarantees.Amount;
                            ServicePrice = (int)Order.Customers.Services.ServicePrice;
                            if (amount > 0 || ServicePrice > 0)
                            {
                                if(Surveys.Guarantees.IsDeposit ==false)
                                {
                                    payment.PaymentAmount = amount + ServicePrice;
                                }
                                else
                                {
                                    payment.PaymentAmount = ServicePrice;
                                }
                            }
                        }
                        payment.AccountRefId = account.AccountId;
                        context.PaymentModels.Add(payment);
                        await context.SaveChangesAsync();
                       
                    }                  
                }
                else
                {
                    Order.UpdatedDate = DateTime.Now;
                    context.orderDetailModels.Update(Order);
                    await context.SaveChangesAsync();
                }
            }
            else
            {
                throw new OrderDetailException("Order Null ");

            }
        }
    }
}