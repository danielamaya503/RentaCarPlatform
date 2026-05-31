using RentaCarPlatform.Models.SIS;
using System;
using System.Collections.Generic;

namespace RentaCarPlatform.Models.ALQ;

public partial class HistorialPrecio
{
    public int HistorialPrecioId { get; set; }

    public int VehiculoId { get; set; }

    public decimal PrecioAnterior { get; set; }

    public decimal PrecioNuevo { get; set; }

    public DateTime FechaCambio { get; set; }

    public int? CambiadoPorUsuarioId { get; set; }

    public string? Motivo { get; set; }

    public virtual Usuario? CambiadoPorUsuario { get; set; }

    public virtual Vehiculo Vehiculo { get; set; } = null!;
}
