using Microsoft.EntityFrameworkCore;
using NexusApp.Areas.Financial.Models;
using NexusApp.Data;
using PayPal.Api;

namespace NexusApp.Areas.Financial.Reposetory.Connect
{
    public class ConnectinImp : IConnectionReposetory
    {
        private readonly ApplicationDbContext context;
        public ConnectinImp(ApplicationDbContext _context)
        {
            context = _context;
        }
        public class ConnectionException : Exception
        {
            public ConnectionException(string message) : base(message) { }
        }
        public Task CreateConnection(ConnectionModel connection)
        {
            throw new NotImplementedException();
        }

        public Task DeteleConnection(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ConnectionModel>> GetAllConnect()
        {
            var conlist = await context.connectionModels.Include(s=>s.Services)
                .Include(o=>o.OrderDetails).ThenInclude(cu=>cu.Customers).ToListAsync();
            if (conlist != null)
            {
                return conlist;
            }
            else
            {
                throw new ConnectionException("Cant Get connect List ");

            }

        }

        public async Task<ConnectionModel> GetConnectbyId(int id)
        {
            var con = await context.connectionModels.SingleOrDefaultAsync( c=>c.ConnectionID ==id);
            if (con != null)
            {
                return con;
            }
            else
            {
                throw new ConnectionException("Cant Get connect with ID : "+id);

            }
        }

        public async Task UpdateConnection(ConnectionModel connection, int? time)
        {
            var con = await GetConnectbyId(connection.ConnectionID);
            var ser = await context.serviceModels.SingleOrDefaultAsync(c => c.ServiceId == con.ServiceRefId);
            if (con != null && con.OrderDetails != null)
            {
                if(time==0)
                {
                    con.Status = "false"!;
                    context.connectionModels.Update(con);
                    var price = ser.ServicePrice;
                    foreach (var item in con.OrderDetails)
                    {
                        var payment = await context.PaymentModels.SingleOrDefaultAsync(c => c.OrderDetailRefId == item.OrderDetailId);
                        payment.PaymentAmount = price;
                        payment.PaymentMode = "Limited";
                        context.PaymentModels.Update(payment);
                        var result = await context.SaveChangesAsync();
                        if (result != null)
                        {
                            var acc = await context.accountModels.SingleOrDefaultAsync(a => a.AccountId == payment.AccountRefId);
                            acc.Status = "false";
                            context.accountModels.Update(acc);
                            await context.SaveChangesAsync();
                        }
                      

                    }
                    await context.SaveChangesAsync();
                }
                else
                {
                    con.Status = "true"!;
                }
            }
        }
    }
}
