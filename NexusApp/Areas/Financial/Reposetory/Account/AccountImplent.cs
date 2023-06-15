using Microsoft.EntityFrameworkCore;
using MimeKit.Cryptography;
using NexusApp.Areas.Customer.Models;
using NexusApp.Areas.Financial.Reposetory.Payment;
using NexusApp.Data;
using static NexusApp.Areas.Financial.Reposetory.Install.Installimp;

namespace NexusApp.Areas.Financial.Reposetory.Account
{
    public class AccountImplent : IAccountReposetory
    {
        private readonly ApplicationDbContext context;
        private readonly IPaymentReposetory paymentReposetory;
        public AccountImplent(ApplicationDbContext _context,IPaymentReposetory _paymentReposetory)
        {
            context = _context;
            paymentReposetory = _paymentReposetory;
        }
        public class AccountException : Exception
        {
            public AccountException(string message) : base(message) { }
        }
        public Task AddAccount(AccountModel account)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAccount(int id)
        {
            var acc = await context.accountModels.FirstOrDefaultAsync(c => c.AccountId == id);
            if (acc != null)
            {
                context.accountModels.Remove(acc);
                context.SaveChanges();              
            }
            else
            {
                throw new AccountException("Cant find Account with ID : " + id);
            }
        }

        public async Task<AccountModel> GetAccountByID(int id)
        {
            var acc = await context.accountModels.Include(c => c.Customer).FirstOrDefaultAsync(c => c.AccountId == id);
            if (acc != null)
            {
                return acc;
            }
            else
            {
                throw new AccountException("Cant find Account with ID : " + id);
            }
        }
        public async Task<List<AccountModel>> GetAllAcount()
        {
            var acc = await context.accountModels.Include(c=>c.Customer).Include(p=>p.Payments).ToListAsync();
            if (acc != null)
            {
                return acc;
            }
            else
            {
                throw new AccountException("List Account is Empty");       
            }
        }
        public Task UpdateAccount(AccountModel account)
        {
            throw new NotImplementedException();
        }
    }
}
