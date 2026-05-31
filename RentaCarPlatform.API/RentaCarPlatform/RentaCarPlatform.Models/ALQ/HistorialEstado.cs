using RentaCarPlatform.Models.SIS;
using System;
using System.Collections.Generic;

namespace RentaCarPlatform.Models.ALQ;

public partial class HistorialEstado
{
    public int HistorialEstadoId { get; set; }

    public int VehiculoId { get; set; }

    public int? EstadoAnteriorId { get; set; }

    public int EstadoNuevoId { get; set; }

    public DateTime FechaCambio { get; set; }

    public string? Motivo { get; set; }

    public int? CambiadoPorUsuarioId { get; set; }

    public virtual Usuario? CambiadoPorUsuario { get; set; }

    public virtual EstadosVehiculo? EstadoAnterior { get; set; }

    public virtual EstadosVehiculo EstadoNuevo { get; set; } = null!;

    public virtual Vehiculo Vehiculo { get; set; } = null!;
}
