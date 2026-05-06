using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreDine.ViewModels
{
    public class AddOrderItemViewModel
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Please select a menu item")]
        [Display(Name = "Menu Item")]
        public int MenuItemId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }


        public IEnumerable<SelectListItem> AvailableItems { get; set; } = new List<SelectListItem>();
    }
}
