using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.ViewModels.ALQ.Response
{
    public class ModeloResponse
    {
        public int ModeloId { get; set; }
        public int MarcaId { get; set; }
        public string MarcaNombre { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}
