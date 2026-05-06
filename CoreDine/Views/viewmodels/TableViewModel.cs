using System.ComponentModel.DataAnnotations;
using CoreDine.Models;

namespace CoreDine.ViewModels
{
    public class TableViewModel
    {
        public int TableId { get; set; }

        [Required]
        [Display(Name = "Table Number")]
        public int TableNumber { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "Capacity must be between 1 and 20")]
        public int Capacity { get; set; }

        public TableStatus Status { get; set; } = TableStatus.Available;
    }
}
