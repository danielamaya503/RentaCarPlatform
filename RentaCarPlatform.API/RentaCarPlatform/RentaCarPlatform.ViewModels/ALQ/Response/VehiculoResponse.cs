using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.ViewModels.ALQ.Response
{
    public class VehiculoResponse
    {
        public int VehiculoId { get; set; }
        public int ModeloId { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int TipoVehiculoId { get; set; }
        public string TipoVehiculo { get; set; } = string.Empty;
        public int EstadoVehiculoId { get; set; }
        public string EstadoNombre { get; set; } = string.Empty;
        public string EstadoColorHex { get; set; } = string.Empty;
        public int Anio { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Transmision { get; set; } = string.Empty;
        public string Combustible { get; set; } = string.Empty;
        public int CapacidadPasajeros { get; set; }
        public decimal PrecioDiario { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public List<CaracteristicaResponse> Caracteristicas { get; set; } = [];
        public List<VehiculoImagenResponse> Imagenes { get; set; } = [];
    }
}
