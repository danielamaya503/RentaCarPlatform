using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentaCarPlatform.ViewModels.ALQ.Request
{
    public class CrearModeloRequest
    {
        [Required(ErrorMessage = "La marca es obligatoria.")]
        public int MarcaId { get; set; }

        [Required(ErrorMessage = "El nombre del modelo es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;
    }
}
