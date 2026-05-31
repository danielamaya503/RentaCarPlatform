using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RentaCarPlatform.Helpers
{
    public static class ClaimsExtensions
    {
        public static int GetUsuarioId(this ClaimsPrincipal user)
        {
            var value = user.FindFirstValue("usuarioId") ?? "0";
            return int.TryParse(value, out var id) ? id : 0;
        }

        public static string GetEmail(this ClaimsPrincipal user)
            => user.FindFirstValue(JwtRegisteredClaimNames.Email) ?? string.Empty;

        public static string GetNombreUsuario(this ClaimsPrincipal user)
            => user.FindFirstValue("name") ?? string.Empty;

        public static string GetRol(this ClaimsPrincipal user)
            => user.FindFirstValue("role") ?? string.Empty;
    }
}
