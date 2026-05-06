using CoreDine.Data;
using CoreDine.Models;
using CoreDine.ViewModels;
using Microsoft.EntityFrameworkCore;
namespace CoreDine.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllOrdersAsync(OrderStatus? status = null, DateTime? date = null)
        {
            var query = _context.Orders
                .Include(o => o.Table)
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(o => o.Status == status.Value);

            if (date.HasValue)
                query = query.Where(o => o.DateCreated.Date == date.Value.Date);

            return await query.ToListAsync();
        }

        public async Task<List<Order>> GetTodaysOrdersAsync()
        {
            var today = DateTime.Today;
            return await _context.Orders
                .Include(o => o.Table)
                .Where(o => o.DateCreated.Date == today)
                .ToListAsync();
        }

        public async Task<Order> GetOrderWithItemsAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Table)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) throw new Exception("Order not found");
            return order;
        }

        public async Task<int> CreateOrderAsync(Order order)
        {
            order.DateCreated = DateTime.Now;
            order.Status = OrderStatus.Pending;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order.OrderId; 
        }

        public async Task AddItemToOrderAsync(int orderId, OrderItem item)
        {
            var menuItem = await _context.MenuItems.FindAsync(item.MenuItemId);
            if (menuItem == null) throw new Exception("Menu item not found");
            if (!menuItem.IsAvailable) throw new Exception("This item is not available");

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) throw new Exception("Order not found");

            item.OrderId = orderId;
            item.UnitPrice = menuItem.Price; 
            item.Snapshot();                 

            _context.OrderItems.Add(item);

            order.FinalizeOrder(); 
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) throw new Exception("Order not found");

            order.Status = status;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) throw new Exception("Order not found");

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
        public async Task<DashboardViewModel> GetDashboardStatsAsync()
        {
            var today = DateTime.Today;

            return new DashboardViewModel
            {
                TodayOrderCount = await _context.Orders
                    .CountAsync(o => o.DateCreated.Date == today),

                TodayRevenue = await _context.Orders
                    .Where(o => o.DateCreated.Date == today)
                    .SumAsync(o => o.TotalAmount),

                ActiveTableCount = await _context.DiningTables
                    .CountAsync(t => t.Status == TableStatus.Occupied),

                AvailableMenuItemCount = await _context.MenuItems
                    .CountAsync(m => m.IsAvailable)
            };
        }
    }
}
