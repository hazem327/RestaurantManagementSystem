using System.ComponentModel.DataAnnotations;
using CoreDine.Models;

namespace CoreDine.ViewModels
{
    public class UpdateOrderStatusViewModel
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; }

        [Required]
        public OrderStatus Status { get; set; }
    }
}
