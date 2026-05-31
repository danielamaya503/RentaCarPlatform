using System;
using System.Collections.Generic;

namespace RentaCarPlatform.Models.ALQ;

public partial class VwHistorialPrecio
{
    public int VehiculoId { get; set; }

    public string Marca { get; set; } = null!;

    public string Modelo { get; set; } = null!;

    public string Placa { get; set; } = null!;

    public decimal PrecioAnterior { get; set; }

    public decimal PrecioNuevo { get; set; }

    public DateTime FechaCambio { get; set; }

    public string? Motivo { get; set; }

    public string? CambiadoPor { get; set; }
}
