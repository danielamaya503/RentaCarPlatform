using System;
using System.Collections.Generic;

namespace RentaCarPlatform.Models.ALQ;

public partial class VwHistorialEstado
{
    public int VehiculoId { get; set; }

    public string Marca { get; set; } = null!;

    public string Modelo { get; set; } = null!;

    public string Placa { get; set; } = null!;

    public string? EstadoAnterior { get; set; }

    public string EstadoNuevo { get; set; } = null!;

    public DateTime FechaCambio { get; set; }

    public string? Motivo { get; set; }

    public string? CambiadoPor { get; set; }
}
