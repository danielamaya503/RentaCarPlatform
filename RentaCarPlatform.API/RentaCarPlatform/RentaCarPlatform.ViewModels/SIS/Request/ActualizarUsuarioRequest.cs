using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.ViewModels.SIS.Request
{
    public class ActualizarUsuarioRequest
    {
        public int UsuarioId { get; set; }
        public int RolId { get; set; }
        public string NombreUsuario { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool Activo { get; set; }
    }
}
