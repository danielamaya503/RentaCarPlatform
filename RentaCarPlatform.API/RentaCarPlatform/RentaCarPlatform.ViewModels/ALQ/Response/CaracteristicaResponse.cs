using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.ViewModels.ALQ.Response
{
    public class CaracteristicaResponse
    {
        public int CaracteristicaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Icono { get; set; }
        public bool Activo { get; set; }
    }
}
