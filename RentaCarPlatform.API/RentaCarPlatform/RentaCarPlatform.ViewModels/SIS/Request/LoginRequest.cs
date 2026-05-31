using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.ViewModels.SIS.Request
{
    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
    }
}
