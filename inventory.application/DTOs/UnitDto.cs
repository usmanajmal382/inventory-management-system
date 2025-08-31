using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.DTOs
{
    public record UnitDto(
      int Id,
      string Name,
      string? Abbreviation,
      bool IsActive,
      int ProductCount);

    public record CreateUnitDto(
        string Name,
        string? Abbreviation);

    public record UpdateUnitDto(
        string Name,
        string? Abbreviation,
        bool IsActive);
}
