using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.ViewModels.SIS.Response
{
    public class UsuarioResponse
    {
        public int UsuarioId { get; set; }
        public int RolId { get; set; }
        public string RolNombre { get; set; } = null!;
        public string NombreUsuario { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool Activo { get; set; }
        public DateTime? UltimoAcceso { get; set; }
        public int? CreadoPorUsuarioId { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
