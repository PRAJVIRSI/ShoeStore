using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoeStore_Group9.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
