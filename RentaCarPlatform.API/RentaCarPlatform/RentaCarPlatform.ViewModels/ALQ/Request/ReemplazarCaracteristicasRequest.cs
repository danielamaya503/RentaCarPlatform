using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentaCarPlatform.ViewModels.ALQ.Request
{
    public class ReemplazarCaracteristicasRequest
    {
        [Required(ErrorMessage = "El identificador del vehículo es obligatorio.")]
        public int VehiculoId { get; set; }

        public List<int> CaracteristicaIds { get; set; } = new();
    }
}
