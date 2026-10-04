
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentaCarPlatform.ViewModels.ALQ.Request
{
    public class AsignarCaracteristicasRequest
    {
        [Required(ErrorMessage = "El identificador del vehículo es obligatorio.")]
        public int VehiculoId { get; set; }

        [Required(ErrorMessage = "Debe enviar al menos una característica.")]
        [MinLength(1, ErrorMessage = "Debe enviar al menos una característica.")]
        public List<int> CaracteristicaIds { get; set; } = new();
    }
}
