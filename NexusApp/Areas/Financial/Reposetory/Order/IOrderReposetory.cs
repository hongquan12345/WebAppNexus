using NexusApp.Areas.Financial.Models;

namespace NexusApp.Areas.Financial.Reposetory.Order
{
    public interface IOrderReposetory
    {
        Task<List<OrderDetailModel>> GetAllOrderDetail();
        Task<OrderDetailModel> GetOrderDetailByID(int id);
        Task AddOrderDetail(OrderDetailModel orderDetail);
        Task UpdateOrderDetail(OrderDetailModel OrderDetail);
        Task DeleteOrderDetail(int id);
    }
}
