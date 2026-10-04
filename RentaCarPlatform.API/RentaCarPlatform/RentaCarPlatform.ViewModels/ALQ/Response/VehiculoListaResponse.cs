using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.ViewModels.ALQ.Response
{
    public class VehiculoListaResponse
    {
        public int VehiculoId { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string TipoVehiculo { get; set; } = string.Empty;
        public string EstadoNombre { get; set; } = string.Empty;
        public string EstadoColorHex { get; set; } = string.Empty;
        public int Anio { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Transmision { get; set; } = string.Empty;
        public string Combustible { get; set; } = string.Empty;
        public int CapacidadPasajeros { get; set; }
        public decimal PrecioDiario { get; set; }
        public string? ImagenPrincipal { get; set; }
    }
}
