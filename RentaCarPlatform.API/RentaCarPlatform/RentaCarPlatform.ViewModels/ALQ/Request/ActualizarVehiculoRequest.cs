using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentaCarPlatform.ViewModels.ALQ.Request
{
    public class ActualizarVehiculoRequest
    {
        [Required(ErrorMessage = "El identificador del vehículo es obligatorio.")]
        public int VehiculoId { get; set; }

        [Required(ErrorMessage = "El modelo es obligatorio.")]
        public int ModeloId { get; set; }

        [Required(ErrorMessage = "El tipo de vehículo es obligatorio.")]
        public int TipoVehiculoId { get; set; }

        [Required(ErrorMessage = "El estado del vehículo es obligatorio.")]
        public int EstadoVehiculoId { get; set; }

        [Required(ErrorMessage = "El año es obligatorio.")]
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100.")]
        public int Anio { get; set; }

        [Required(ErrorMessage = "La placa es obligatoria.")]
        [MaxLength(20)]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "El color es obligatorio.")]
        [MaxLength(30)]
        public string Color { get; set; } = string.Empty;

        [Required(ErrorMessage = "La transmisión es obligatoria.")]
        [RegularExpression("^(Automatico|Manual)$",
            ErrorMessage = "La transmisión debe ser 'Automatico' o 'Manual'.")]
        public string Transmision { get; set; } = string.Empty;

        [Required(ErrorMessage = "El combustible es obligatorio.")]
        [RegularExpression("^(Gasolina|Diesel|Hibrido|Electrico)$",
            ErrorMessage = "El combustible debe ser: Gasolina, Diesel, Hibrido o Electrico.")]
        public string Combustible { get; set; } = string.Empty;

        [Required(ErrorMessage = "La capacidad de pasajeros es obligatoria.")]
        [Range(1, 99)]
        public int CapacidadPasajeros { get; set; }

        [Required(ErrorMessage = "El precio diario es obligatorio.")]
        [Range(0.01, double.MaxValue)]
        public decimal PrecioDiario { get; set; }

        public string? Descripcion { get; set; }

        public List<int> CaracteristicaIds { get; set; } = [];
    }
}
