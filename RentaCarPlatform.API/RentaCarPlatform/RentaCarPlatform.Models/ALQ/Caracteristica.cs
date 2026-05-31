using System;
using System.Collections.Generic;


namespace RentaCarPlatform.Models.ALQ;

public partial class Caracteristica
{
    public int CaracteristicaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Icono { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
