using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentaCarPlatform.ViewModels.ALQ.Request
{
    public class ActualizarTipoVehiculoRequest
    {
        [Required(ErrorMessage = "El identificador del tipo de vehículo es obligatorio.")]
        public int TipoVehiculoId { get; set; }

        [Required(ErrorMessage = "El nombre del tipo de vehículo es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        public bool Activo { get; set; }
    }
}
