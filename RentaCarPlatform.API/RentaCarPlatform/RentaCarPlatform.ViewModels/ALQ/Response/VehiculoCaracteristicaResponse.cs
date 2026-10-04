using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.ViewModels.ALQ.Response
{
    public class VehiculoCaracteristicaResponse
    {
        public int VehiculoId { get; set; }
        public int CaracteristicaId { get; set; }
        public string CaracteristicaNombre { get; set; } = string.Empty;
    }
}
