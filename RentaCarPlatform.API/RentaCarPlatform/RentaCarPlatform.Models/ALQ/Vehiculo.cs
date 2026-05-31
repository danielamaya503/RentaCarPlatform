using RentaCarPlatform.Models.SIS;
using System;
using System.Collections.Generic;

namespace RentaCarPlatform.Models.ALQ;

public partial class Vehiculo
{
    public int VehiculoId { get; set; }

    public int ModeloId { get; set; }

    public int TipoVehiculoId { get; set; }

    public int EstadoVehiculoId { get; set; }

    public int CreadoPorUsuarioId { get; set; }

    public int Anio { get; set; }

    public string Placa { get; set; } = null!;

    public string Color { get; set; } = null!;

    public string Transmision { get; set; } = null!;

    public string Combustible { get; set; } = null!;

    public int CapacidadPasajeros { get; set; }

    public decimal PrecioDiario { get; set; }

    public string? Descripcion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaActualizacion { get; set; }

    public virtual Usuario CreadoPorUsuario { get; set; } = null!;

    public virtual ICollection<DisponibilidadVehiculo> DisponibilidadVehiculos { get; set; } = new List<DisponibilidadVehiculo>();

    public virtual EstadosVehiculo EstadoVehiculo { get; set; } = null!;

    public virtual ICollection<HistorialEstado> HistorialEstados { get; set; } = new List<HistorialEstado>();

    public virtual ICollection<HistorialPrecio> HistorialPrecios { get; set; } = new List<HistorialPrecio>();

    public virtual Modelo Modelo { get; set; } = null!;

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual TiposVehiculo TipoVehiculo { get; set; } = null!;

    public virtual ICollection<VehiculoImagene> VehiculoImagenes { get; set; } = new List<VehiculoImagene>();

    public virtual ICollection<Caracteristica> Caracteristicas { get; set; } = new List<Caracteristica>();
}
