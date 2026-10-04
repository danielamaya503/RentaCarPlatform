using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.ViewModels.ALQ.Response
{
    public class VehiculoImagenResponse
    {
        public int ImagenId { get; set; }
        public string UrlImagen { get; set; } = string.Empty;
        public bool EsPrincipal { get; set; }
        public int Orden { get; set; }
    }

    public class VehiculoImagenResponse2
    {
        public int ImagenId { get; set; }
        public int VehiculoId { get; set; }
        public string UrlImagen { get; set; } = string.Empty;
        public bool EsPrincipal { get; set; }
        public int Orden { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
