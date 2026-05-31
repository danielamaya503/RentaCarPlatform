using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.ViewModels.SIS.Response
{
    public class RolResponse
    {
        public int RolId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
