using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentaCarPlatform.ViewModels.ALQ.Request
{
    public class CrearVehiculoImagenesRequest
    {
        [Required(ErrorMessage = "El identificador del vehículo es obligatorio.")]
        public int VehiculoId { get; set; }

        [Required(ErrorMessage = "Debe enviar al menos una URL de imagen.")]
        [MinLength(1, ErrorMessage = "Debe enviar al menos una URL de imagen.")]
        public List<UrlImagenItem> Imagenes { get; set; } = new();
    }

    public class UrlImagenItem
    {
        [Required(ErrorMessage = "La URL de la imagen es obligatoria.")]
        [MaxLength(500, ErrorMessage = "La URL no puede exceder 500 caracteres.")]
        [Url(ErrorMessage = "La URL no es válida.")]
        public string UrlImagen { get; set; } = string.Empty;

        public bool EsPrincipal { get; set; } = false;

        public int? Orden { get; set; }
    }
}
