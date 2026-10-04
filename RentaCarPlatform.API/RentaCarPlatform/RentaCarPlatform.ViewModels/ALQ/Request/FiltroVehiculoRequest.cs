using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.ViewModels.ALQ.Request
{
    public class FiltroVehiculoRequest
    {
        // Búsqueda libre por Marca + Modelo
        public string? Busqueda { get; set; }

        // Filtros de catálogo
        public int? MarcaId { get; set; }
        public int? ModeloId { get; set; }
        public int? TipoVehiculoId { get; set; }
        public string? Transmision { get; set; }
        public string? Combustible { get; set; }
        public int? CapacidadPasajeros { get; set; }
        public int? AnioDesde { get; set; }
        public int? AnioHasta { get; set; }
        public decimal? PrecioMinimo { get; set; }
        public decimal? PrecioMaximo { get; set; }

        // Paginación
        public int Pagina { get; set; } = 1;
        public int TamanoPagina { get; set; } = 12;
    }
}
