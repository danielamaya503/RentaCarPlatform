using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.ViewModels.SIS.Request
{
    public class CambiarContrasenaRequest
    {
        public int UsuarioId { get; set; }
        public string NuevaContrasena { get; set; } = null!;
    }
}
