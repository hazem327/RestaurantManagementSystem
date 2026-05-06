using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreDine.ViewModels
{
    public class CreateOrderViewModel
    {
        [Required(ErrorMessage = "Please select a table")]
        [Display(Name = "Table")]
        public int TableId { get; set; }

        [Required]
        [Display(Name = "Order Name")]
        public string OrderName { get; set; }

        
        public IEnumerable<SelectListItem> Tables { get; set; } = new List<SelectListItem>();
    }
}
