using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.core.Entities
{
    public class ProductSupplier
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public decimal SupplierPrice { get; set; }
        public bool IsPreferredSupplier { get; set; }
        public int LeadTimeDays { get; set; }
    }
}
