using System;
using System.Collections.Generic;

namespace RentaCarPlatform.Models.ALQ;

public partial class EstadosVehiculo
{
    public int EstadoVehiculoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? ColorHex { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<HistorialEstado> HistorialEstadoEstadoAnteriors { get; set; } = new List<HistorialEstado>();

    public virtual ICollection<HistorialEstado> HistorialEstadoEstadoNuevos { get; set; } = new List<HistorialEstado>();

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
