using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentaCarPlatform.ViewModels.ALQ.Request
{
    public class ActualizarMarcaRequest
    {
        [Required(ErrorMessage = "El identificador de la marca es obligatorio.")]
        public int MarcaId { get; set; }

        [Required(ErrorMessage = "El nombre de la marca es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        public bool Activo { get; set; }
    }
}
