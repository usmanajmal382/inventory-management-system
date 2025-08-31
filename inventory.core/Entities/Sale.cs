using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.core.Entities
{

    public class Sale
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string SaleNumber { get; set; } = default!;

        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        [StringLength(200)]
        public string? CustomerName { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }

        public int? OrderId { get; set; }
        public Order? Order { get; set; }

        public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
    }

}
