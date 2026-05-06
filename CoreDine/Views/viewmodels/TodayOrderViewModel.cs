using CoreDine.Models;

namespace CoreDine.ViewModels
{
    public class TodayOrderViewModel
    {
        public int OrderId { get; set; }
        public int TableNumber { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
