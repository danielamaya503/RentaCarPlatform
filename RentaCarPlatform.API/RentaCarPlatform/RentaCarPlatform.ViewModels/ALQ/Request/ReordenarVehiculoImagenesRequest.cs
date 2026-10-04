using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentaCarPlatform.ViewModels.ALQ.Request
{
    public class ReordenarVehiculoImagenesRequest
    {
        [Required(ErrorMessage = "El identificador del vehículo es obligatorio.")]
        public int VehiculoId { get; set; }

        /// <summary>
        /// Lista de pares ImagenId-Orden.
        /// </summary>
        [Required]
        public List<ImagenOrdenItem> Imagenes { get; set; } = new();
    }

    public class ImagenOrdenItem
    {
        public int ImagenId { get; set; }
        public int Orden { get; set; }
    }
}
