using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.core.Entities
{
    public class Unit
    {
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string Name { get; set; } = default!; // e.g., "Piece", "Kg", "Liter"

        [StringLength(10)]
        public string? Abbreviation { get; set; } // e.g., "pc", "kg", "ltr"

        public bool IsActive { get; set; } = true;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
