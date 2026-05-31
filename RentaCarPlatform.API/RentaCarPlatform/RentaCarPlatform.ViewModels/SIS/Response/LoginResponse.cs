using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.ViewModels.SIS.Response
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public string NombreUsuario { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public DateTime Expiracion { get; set; }
    }
}
