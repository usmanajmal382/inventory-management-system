using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.core.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required, StringLength(100)] public string Name { get; set; } = default!;
        [StringLength(500)] public string? Description { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }

}
