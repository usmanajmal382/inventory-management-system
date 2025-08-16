using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.core.Entities
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required, StringLength(200)] public string Name { get; set; } = default!;
        [StringLength(100)] public string? ContactPerson { get; set; }
        [StringLength(20)] public string? Phone { get; set; }
        [StringLength(100)] public string? Email { get; set; }
        [StringLength(500)] public string? Address { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
        public ICollection<ProductSupplier> ProductSuppliers { get; set; } = new List<ProductSupplier>();
    }
}
