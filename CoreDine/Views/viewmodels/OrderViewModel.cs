using CoreDine.Models;

namespace CoreDine.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; }
        public int TableNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();
    }
}
