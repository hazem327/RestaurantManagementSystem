using CoreDine.Models;
using CoreDine.ViewModels;

namespace CoreDine.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrdersAsync(OrderStatus? status = null, DateTime? date = null);
        Task<List<Order>> GetTodaysOrdersAsync();
        Task<Order> GetOrderWithItemsAsync(int id);
        Task<int> CreateOrderAsync(Order order);
        Task AddItemToOrderAsync(int orderId, OrderItem item);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task DeleteOrderAsync(int id);
        Task<DashboardViewModel> GetDashboardStatsAsync();
    }
}
