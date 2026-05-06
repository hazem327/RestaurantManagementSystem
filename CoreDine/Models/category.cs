using System.ComponentModel.DataAnnotations;

namespace CoreDine.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required, StringLength(60)]
        public string Name { get; set; }

        public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
