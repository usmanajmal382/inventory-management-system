using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.DTOs
{

    public record CategoryDto(int Id, string Name, string? Description, int ProductCount);

}
