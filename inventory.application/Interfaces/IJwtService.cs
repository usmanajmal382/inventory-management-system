using inventory.core.Models;
using System.Security.Claims;

namespace inventory.application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        ClaimsPrincipal? ValidateToken(string token);
        DateTime GetTokenExpiration();
    }

}
