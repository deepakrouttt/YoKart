using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Extensions
{

    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal user) => user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public static string? GetUsername(this ClaimsPrincipal user) => user?.FindFirst(ClaimTypes.Name)?.Value;

        public static string? GetEmail(this ClaimsPrincipal user) => user?.FindFirst("Email")?.Value;

        public static string? GetRoles(this ClaimsPrincipal user) => user?.FindFirst("Roles")?.Value;
    }

}
