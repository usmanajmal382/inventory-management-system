using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.DTOs
{
    public class CreateSupplierDto
    {
        [Required, StringLength(200)] public string Name { get; set; } = default!;
        [StringLength(100)] public string? ContactPerson { get; set; }
        [StringLength(20)] public string? Phone { get; set; }
        [EmailAddress, StringLength(100)] public string? Email { get; set; }
        [StringLength(500)] public string? Address { get; set; }
    }

}
