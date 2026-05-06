using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreDine.Models
{
    public enum OrderStatus
    {
        Pending,
        Preparing,
        Served,
        Paid,
        Cancelled
    }

    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public string OrderName { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; private set; }

        [Required]
        public int TableId { get; set; }

        [ForeignKey("TableId")]
        public virtual DiningTable Table { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public decimal GetCalculatedTotal() => OrderItems.Sum(i => i.SubTotal);

        public void FinalizeOrder()
        {
            TotalAmount = GetCalculatedTotal();
            Status = OrderStatus.Pending;
        }
    }
}
