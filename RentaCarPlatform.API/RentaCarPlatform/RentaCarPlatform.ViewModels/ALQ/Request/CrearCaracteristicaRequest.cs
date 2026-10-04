using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentaCarPlatform.ViewModels.ALQ.Request
{
    public class CrearCaracteristicaRequest
    {
        [Required(ErrorMessage = "El nombre de la característica es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "El icono no puede exceder 50 caracteres.")]
        public string? Icono { get; set; }

        public bool Activo { get; set; } = true;
    }
}
