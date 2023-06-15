using NexusApp.Areas.Financial.Models;
using NexusApp.ModelDTOs;

namespace NexusApp.Areas.Financial.Reposetory.Payment
{
    public interface IPaymentReposetory
    {
        Task<List<PaymentModel>> GetAllPayment();
        Task<Dictionary<string, decimal>> DashBoard(string sortBy);
        Task<List<MonthData>> GetMonthlyData();
        Task<PaymentModel> GetPaymentById(int id);
        Task UpdatePayment(PaymentModel paymentform,string status);
        Task CreatePayment(PaymentModel paymentform);
        Task DeletePayment(int id);
        Task<PaymentModel> GetPaymentByCusId(int id);


    }
}
