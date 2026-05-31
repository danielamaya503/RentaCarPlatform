using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.ViewModels.ALQ.Response
{
    public class EstadoVehiculoResponse
    {
        public int EstadoVehiculoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? ColorHex { get; set; }
        public bool Activo { get; set; }
    }
}
