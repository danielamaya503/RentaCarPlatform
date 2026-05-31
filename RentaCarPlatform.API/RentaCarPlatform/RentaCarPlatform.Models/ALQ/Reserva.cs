using RentaCarPlatform.Models.SIS;
using System;
using System.Collections.Generic;

namespace RentaCarPlatform.Models.ALQ;

public partial class Reserva
{
    public int ReservaId { get; set; }

    public int VehiculoId { get; set; }

    public int ClienteId { get; set; }

    public int CreadoPorUsuarioId { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public string Estado { get; set; } = null!;

    public decimal? PrecioTotal { get; set; }

    public string? MetodoPago { get; set; }

    public string? Notas { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaActualizacion { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual Usuario CreadoPorUsuario { get; set; } = null!;

    public virtual Vehiculo Vehiculo { get; set; } = null!;
}
