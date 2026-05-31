using System;
using System.Collections.Generic;

namespace RentaCarPlatform.Models.ALQ;

public partial class VwReservasActiva
{
    public int ReservaId { get; set; }

    public string Marca { get; set; } = null!;

    public string Modelo { get; set; } = null!;

    public string Placa { get; set; } = null!;

    public string Cliente { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string? Email { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public decimal? PrecioTotal { get; set; }

    public string Estado { get; set; } = null!;

    public string? MetodoPago { get; set; }
}
