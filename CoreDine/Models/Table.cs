using System.ComponentModel.DataAnnotations;

namespace CoreDine.Models
{
    public enum TableStatus { Available, Occupied, Reserved, Dirty }

    public class DiningTable
    {
        [Key]
        public int TableId { get; set; }

        [Required]
        public int TableNumber { get; set; }

        [Required]
        public int Capacity { get; set; }

        public TableStatus Status { get; set; } = TableStatus.Available;

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
