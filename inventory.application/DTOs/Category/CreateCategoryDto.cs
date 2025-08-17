using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.DTOs
{
    public class CreateCategoryDto
    {
        [Required, StringLength(100)] public string Name { get; set; } = default!;
        [StringLength(500)] public string? Description { get; set; }
    }
}
