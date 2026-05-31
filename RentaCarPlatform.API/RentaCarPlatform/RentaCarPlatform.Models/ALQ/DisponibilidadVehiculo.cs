using RentaCarPlatform.Models.SIS;
using System;
using System.Collections.Generic;

namespace RentaCarPlatform.Models.ALQ;

public partial class DisponibilidadVehiculo
{
    public int DisponibilidadId { get; set; }

    public int VehiculoId { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public string? Motivo { get; set; }

    public int CreadoPorUsuarioId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Usuario CreadoPorUsuario { get; set; } = null!;

    public virtual Vehiculo Vehiculo { get; set; } = null!;
}
