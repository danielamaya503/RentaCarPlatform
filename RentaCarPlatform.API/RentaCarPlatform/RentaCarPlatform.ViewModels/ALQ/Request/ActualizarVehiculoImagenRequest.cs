using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentaCarPlatform.ViewModels.ALQ.Request
{
    public class ActualizarVehiculoImagenRequest
    {
        [Required(ErrorMessage = "Debe enviar al menos una imagen para actualizar.")]
        [MinLength(1, ErrorMessage = "Debe enviar al menos una imagen para actualizar.")]
        public List<VehiculoItem> Vehiculos { get; set; } = new ();
    }

    public class VehiculoItem
    {
        [Required(ErrorMessage = "El identificador de la imagen es obligatorio.")]
        public int ImagenId { get; set; }

        [Required(ErrorMessage = "La URL de la imagen es obligatoria.")]
        [MaxLength(500, ErrorMessage = "La URL no puede exceder 500 caracteres.")]
        [Url(ErrorMessage = "La URL de la imagen no es válida.")]
        public string UrlImagen { get; set; } = string.Empty;

        public bool EsPrincipal { get; set; }

        public int Orden { get; set; }
    }
}
