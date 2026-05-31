using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentaCarPlatform.ViewModels.SIS.Request
{
    public class CrearUsuarioRequest
    {

        public int RolId { get; set; }
        public string NombreUsuario { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
        public bool Activo { get; set; } = true;
        public int? CreadoPorUsuarioId { get; set; }

    }
}
