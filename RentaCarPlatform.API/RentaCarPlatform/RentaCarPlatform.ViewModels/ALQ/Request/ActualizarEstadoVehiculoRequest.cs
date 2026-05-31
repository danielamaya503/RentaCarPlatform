using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentaCarPlatform.ViewModels.ALQ.Request
{
    public class ActualizarEstadoVehiculoRequest
    {
        [Required(ErrorMessage = "El identificador del estado es obligatorio.")]
        public int EstadoVehiculoId { get; set; }

        [Required(ErrorMessage = "El nombre del estado es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(7, ErrorMessage = "El color hex no puede exceder 7 caracteres.")]
        [RegularExpression(@"^#([A-Fa-f0-9]{6})$", ErrorMessage = "El color debe tener formato hexadecimal válido. Ejemplo: #28a745")]
        public string? ColorHex { get; set; }

        public bool Activo { get; set; }
    }
}
