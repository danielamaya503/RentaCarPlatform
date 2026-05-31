using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentaCarPlatform.ViewModels.SIS.Request
{
    public class ActualizarRolRequest
    {
        [Required]
        public int RolId { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres.")]
        public string? Descripcion { get; set; }

        public bool Activo { get; set; }
    }
}
