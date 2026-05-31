using System;
using System.Collections.Generic;

namespace RentaCarPlatform.Models.ALQ;

public partial class VwHistorialReservasCliente
{
    public int ClienteId { get; set; }

    public string Cliente { get; set; } = null!;

    public string? Dui { get; set; }

    public string Telefono { get; set; } = null!;

    public string? Email { get; set; }

    public string Marca { get; set; } = null!;

    public string Modelo { get; set; } = null!;

    public string Placa { get; set; } = null!;

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public int? DiasRentados { get; set; }

    public decimal? PrecioTotal { get; set; }

    public string Estado { get; set; } = null!;

    public string? MetodoPago { get; set; }

    public DateTime FechaReserva { get; set; }
}
