using System;
using System.Collections.Generic;

namespace RentaCarPlatform.Models.ALQ;

public partial class VehiculoImagene
{
    public int ImagenId { get; set; }

    public int VehiculoId { get; set; }

    public string Urlimagen { get; set; } = null!;

    public bool EsPrincipal { get; set; }

    public int Orden { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Vehiculo Vehiculo { get; set; } = null!;
}
