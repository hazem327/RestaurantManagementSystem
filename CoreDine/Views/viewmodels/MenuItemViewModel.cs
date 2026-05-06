using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreDine.ViewModels
{
    public class MenuItemViewModel
    {
        public int MenuItemId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;

        [Required(ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }

        
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}
