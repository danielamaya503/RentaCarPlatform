using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RentaCarPlatform.Models.ALQ;
using RentaCarPlatform.Models.SIS;


namespace RentaCarPlatform.Concretes;

public partial class RentaCarPlatformContext : DbContext
{
    public RentaCarPlatformContext()
    {
    }

    public RentaCarPlatformContext(DbContextOptions<RentaCarPlatformContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Caracteristica> Caracteristicas { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<DisponibilidadVehiculo> DisponibilidadVehiculos { get; set; }

    public virtual DbSet<EstadosVehiculo> EstadosVehiculos { get; set; }

    public virtual DbSet<HistorialEstado> HistorialEstados { get; set; }

    public virtual DbSet<HistorialPrecio> HistorialPrecios { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<Modelo> Modelos { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TiposVehiculo> TiposVehiculos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

    public virtual DbSet<VehiculoImagene> VehiculoImagenes { get; set; }

    public virtual DbSet<VwHistorialEstado> VwHistorialEstados { get; set; }

    public virtual DbSet<VwHistorialPrecio> VwHistorialPrecios { get; set; }

    public virtual DbSet<VwHistorialReservasCliente> VwHistorialReservasClientes { get; set; }

    public virtual DbSet<VwReservasActiva> VwReservasActivas { get; set; }

    public virtual DbSet<VwVehiculosDisponible> VwVehiculosDisponibles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Caracteristica>(entity =>
        {
            entity.HasKey(e => e.CaracteristicaId).HasName("PK__Caracter__E52941373463E855");

            entity.ToTable("Caracteristicas", "ALQ");

            entity.HasIndex(e => e.Nombre, "UQ__Caracter__75E3EFCFC542A5DD").IsUnique();

            entity.Property(e => e.CaracteristicaId).HasColumnName("CaracteristicaID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Icono)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Clientes__71ABD0A73895FA88");

            entity.ToTable("Clientes", "ALQ");

            entity.HasIndex(e => e.Dui, "IDX_Clientes_DUI");

            entity.HasIndex(e => e.Email, "IDX_Clientes_Email");

            entity.HasIndex(e => e.Email, "UQ__Clientes__A9D1053446BAE1D4").IsUnique();

            entity.HasIndex(e => e.Dui, "UQ__Clientes__C03671B9E2F4C192").IsUnique();

            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreadoPorUsuarioId).HasColumnName("CreadoPorUsuarioID");
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Dui)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DUI");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.CreadoPorUsuario).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.CreadoPorUsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Clientes_Usuario");
        });

        modelBuilder.Entity<DisponibilidadVehiculo>(entity =>
        {
            entity.HasKey(e => e.DisponibilidadId).HasName("PK__Disponib__0300F3D4EB946C5F");

            entity.ToTable("DisponibilidadVehiculo", "ALQ");

            entity.HasIndex(e => new { e.FechaInicio, e.FechaFin }, "IDX_Disponibilidad_Fechas");

            entity.HasIndex(e => e.VehiculoId, "IDX_Disponibilidad_Veh");

            entity.Property(e => e.DisponibilidadId).HasColumnName("DisponibilidadID");
            entity.Property(e => e.CreadoPorUsuarioId).HasColumnName("CreadoPorUsuarioID");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Motivo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.VehiculoId).HasColumnName("VehiculoID");

            entity.HasOne(d => d.CreadoPorUsuario).WithMany(p => p.DisponibilidadVehiculos)
                .HasForeignKey(d => d.CreadoPorUsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Disponibilidad_Usuario");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.DisponibilidadVehiculos)
                .HasForeignKey(d => d.VehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Disponibilidad_Vehiculo");
        });

        modelBuilder.Entity<EstadosVehiculo>(entity =>
        {
            entity.HasKey(e => e.EstadoVehiculoId).HasName("PK__EstadosV__72F520DC9845D1ED");

            entity.ToTable("EstadosVehiculo", "ALQ");

            entity.HasIndex(e => e.Nombre, "UQ__EstadosV__75E3EFCFAE3F32A0").IsUnique();

            entity.Property(e => e.EstadoVehiculoId).HasColumnName("EstadoVehiculoID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.ColorHex)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HistorialEstado>(entity =>
        {
            entity.HasKey(e => e.HistorialEstadoId).HasName("PK__Historia__D497DEBD65C46F1A");

            entity.ToTable("HistorialEstados", "ALQ");

            entity.HasIndex(e => e.VehiculoId, "IDX_HistorialEstados_Veh");

            entity.Property(e => e.HistorialEstadoId).HasColumnName("HistorialEstadoID");
            entity.Property(e => e.CambiadoPorUsuarioId).HasColumnName("CambiadoPorUsuarioID");
            entity.Property(e => e.EstadoAnteriorId).HasColumnName("EstadoAnteriorID");
            entity.Property(e => e.EstadoNuevoId).HasColumnName("EstadoNuevoID");
            entity.Property(e => e.FechaCambio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Motivo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.VehiculoId).HasColumnName("VehiculoID");

            entity.HasOne(d => d.CambiadoPorUsuario).WithMany(p => p.HistorialEstados)
                .HasForeignKey(d => d.CambiadoPorUsuarioId)
                .HasConstraintName("FK_HistorialEstados_Usuario");

            entity.HasOne(d => d.EstadoAnterior).WithMany(p => p.HistorialEstadoEstadoAnteriors)
                .HasForeignKey(d => d.EstadoAnteriorId)
                .HasConstraintName("FK_HistorialEstados_EstAnterior");

            entity.HasOne(d => d.EstadoNuevo).WithMany(p => p.HistorialEstadoEstadoNuevos)
                .HasForeignKey(d => d.EstadoNuevoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialEstados_EstNuevo");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.HistorialEstados)
                .HasForeignKey(d => d.VehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialEstados_Vehiculo");
        });

        modelBuilder.Entity<HistorialPrecio>(entity =>
        {
            entity.HasKey(e => e.HistorialPrecioId).HasName("PK__Historia__BA57B8FE71A8F643");

            entity.ToTable("HistorialPrecios", "ALQ");

            entity.HasIndex(e => e.VehiculoId, "IDX_HistorialPrecios_Veh");

            entity.Property(e => e.HistorialPrecioId).HasColumnName("HistorialPrecioID");
            entity.Property(e => e.CambiadoPorUsuarioId).HasColumnName("CambiadoPorUsuarioID");
            entity.Property(e => e.FechaCambio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Motivo)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PrecioAnterior).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PrecioNuevo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VehiculoId).HasColumnName("VehiculoID");

            entity.HasOne(d => d.CambiadoPorUsuario).WithMany(p => p.HistorialPrecios)
                .HasForeignKey(d => d.CambiadoPorUsuarioId)
                .HasConstraintName("FK_HistorialPrecios_Usuario");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.HistorialPrecios)
                .HasForeignKey(d => d.VehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialPrecios_Vehiculo");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => e.MarcaId).HasName("PK__Marcas__D5B1CDEB5F2EE833");

            entity.ToTable("Marcas", "ALQ");

            entity.HasIndex(e => e.Nombre, "UQ__Marcas__75E3EFCF4A2A6F35").IsUnique();

            entity.Property(e => e.MarcaId).HasColumnName("MarcaID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Modelo>(entity =>
        {
            entity.HasKey(e => e.ModeloId).HasName("PK__Modelos__FA6052BAF29FB0AC");

            entity.ToTable("Modelos", "ALQ");

            entity.HasIndex(e => new { e.MarcaId, e.Nombre }, "UQ_Modelos_MarcaModelo").IsUnique();

            entity.Property(e => e.ModeloId).HasColumnName("ModeloID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.MarcaId).HasColumnName("MarcaID");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Marca).WithMany(p => p.Modelos)
                .HasForeignKey(d => d.MarcaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Modelos_Marca");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.ReservaId).HasName("PK__Reservas__C39937032200479D");

            entity.ToTable("Reservas", "ALQ", tb =>
                {
                    tb.HasTrigger("trg_Reservas_CambiarEstadoVehiculo");
                    tb.HasTrigger("trg_Reservas_InsertUpdate");
                    tb.HasTrigger("trg_Reservas_ValidarTraslape");
                });

            entity.HasIndex(e => e.ClienteId, "IDX_Reservas_Cliente");

            entity.HasIndex(e => e.Estado, "IDX_Reservas_Estado");

            entity.HasIndex(e => new { e.FechaInicio, e.FechaFin }, "IDX_Reservas_Fechas");

            entity.HasIndex(e => e.VehiculoId, "IDX_Reservas_Vehiculo");

            entity.Property(e => e.ReservaId).HasColumnName("ReservaID");
            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.CreadoPorUsuarioId).HasColumnName("CreadoPorUsuarioID");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaActualizacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Notas).IsUnicode(false);
            entity.Property(e => e.PrecioTotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VehiculoId).HasColumnName("VehiculoID");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservas_Cliente");

            entity.HasOne(d => d.CreadoPorUsuario).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.CreadoPorUsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservas_Usuario");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.VehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservas_Vehiculo");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__Roles__F92302D1B1A5F572");

            entity.ToTable("Roles", "SIS");

            entity.HasIndex(e => e.Nombre, "UQ__Roles__75E3EFCF781C3482").IsUnique();

            entity.Property(e => e.RolId).HasColumnName("RolID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TiposVehiculo>(entity =>
        {
            entity.HasKey(e => e.TipoVehiculoId).HasName("PK__TiposVeh__1EA21D2D2AAA1E13");

            entity.ToTable("TiposVehiculo", "ALQ");

            entity.HasIndex(e => e.Nombre, "UQ__TiposVeh__75E3EFCF35B99DFC").IsUnique();

            entity.Property(e => e.TipoVehiculoId).HasColumnName("TipoVehiculoID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE79825DDEBFE");

            entity.ToTable("Usuarios", "SIS");

            entity.HasIndex(e => e.NombreUsuario, "UQ__Usuarios__6B0F5AE074D7454A").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D10534F0C96211").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Contrasena)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreadoPorUsuarioId).HasColumnName("CreadoPorUsuarioID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RolId).HasColumnName("RolID");
            entity.Property(e => e.UltimoAcceso).HasColumnType("datetime");

            entity.HasOne(d => d.CreadoPorUsuario).WithMany(p => p.InverseCreadoPorUsuario)
                .HasForeignKey(d => d.CreadoPorUsuarioId)
                .HasConstraintName("FK_Usuarios_CreadoPor");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Rol");
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(e => e.VehiculoId).HasName("PK__Vehiculo__AA088620B910A6E4");

            entity.ToTable("Vehiculos", "ALQ", tb =>
                {
                    tb.HasTrigger("trg_Vehiculos_Insert");
                    tb.HasTrigger("trg_Vehiculos_Update");
                });

            entity.HasIndex(e => e.EstadoVehiculoId, "IDX_Vehiculos_Estado");

            entity.HasIndex(e => e.ModeloId, "IDX_Vehiculos_Modelo");

            entity.HasIndex(e => e.Placa, "UQ__Vehiculo__8310F99DB261CA79").IsUnique();

            entity.Property(e => e.VehiculoId).HasColumnName("VehiculoID");
            entity.Property(e => e.Color)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Combustible)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreadoPorUsuarioId).HasColumnName("CreadoPorUsuarioID");
            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.EstadoVehiculoId).HasColumnName("EstadoVehiculoID");
            entity.Property(e => e.FechaActualizacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModeloId).HasColumnName("ModeloID");
            entity.Property(e => e.Placa)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PrecioDiario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TipoVehiculoId).HasColumnName("TipoVehiculoID");
            entity.Property(e => e.Transmision)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.CreadoPorUsuario).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.CreadoPorUsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vehiculos_Usuario");

            entity.HasOne(d => d.EstadoVehiculo).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.EstadoVehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vehiculos_Estado");

            entity.HasOne(d => d.Modelo).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.ModeloId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vehiculos_Modelo");

            entity.HasOne(d => d.TipoVehiculo).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.TipoVehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vehiculos_Tipo");

            entity.HasMany(d => d.Caracteristicas).WithMany(p => p.Vehiculos)
                .UsingEntity<Dictionary<string, object>>(
                    "VehiculoCaracteristica",
                    r => r.HasOne<Caracteristica>().WithMany()
                        .HasForeignKey("CaracteristicaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_VehCar_Caracteristica"),
                    l => l.HasOne<Vehiculo>().WithMany()
                        .HasForeignKey("VehiculoId")
                        .HasConstraintName("FK_VehCar_Vehiculo"),
                    j =>
                    {
                        j.HasKey("VehiculoId", "CaracteristicaId");
                        j.ToTable("VehiculoCaracteristicas", "ALQ");
                        j.IndexerProperty<int>("VehiculoId").HasColumnName("VehiculoID");
                        j.IndexerProperty<int>("CaracteristicaId").HasColumnName("CaracteristicaID");
                    });
        });

        modelBuilder.Entity<VehiculoImagene>(entity =>
        {
            entity.HasKey(e => e.ImagenId).HasName("PK__Vehiculo__0C7D20D745960757");

            entity.ToTable("VehiculoImagenes", "ALQ", tb => tb.HasTrigger("trg_VehiculoImagenes_UnaPrincipal"));

            entity.Property(e => e.ImagenId).HasColumnName("ImagenID");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Urlimagen)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("URLImagen");
            entity.Property(e => e.VehiculoId).HasColumnName("VehiculoID");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.VehiculoImagenes)
                .HasForeignKey(d => d.VehiculoId)
                .HasConstraintName("FK_VehiculoImagenes_Vehiculo");
        });

        modelBuilder.Entity<VwHistorialEstado>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_HistorialEstados", "ALQ");

            entity.Property(e => e.CambiadoPor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EstadoAnterior)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EstadoNuevo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaCambio).HasColumnType("datetime");
            entity.Property(e => e.Marca)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Modelo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Motivo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Placa)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.VehiculoId).HasColumnName("VehiculoID");
        });

        modelBuilder.Entity<VwHistorialPrecio>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_HistorialPrecios", "ALQ");

            entity.Property(e => e.CambiadoPor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaCambio).HasColumnType("datetime");
            entity.Property(e => e.Marca)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Modelo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Motivo)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Placa)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PrecioAnterior).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PrecioNuevo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VehiculoId).HasColumnName("VehiculoID");
        });

        modelBuilder.Entity<VwHistorialReservasCliente>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_HistorialReservasCliente", "ALQ");

            entity.Property(e => e.Cliente)
                .HasMaxLength(201)
                .IsUnicode(false);
            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.Dui)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DUI");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FechaReserva).HasColumnType("datetime");
            entity.Property(e => e.Marca)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Modelo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Placa)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PrecioTotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwReservasActiva>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ReservasActivas", "ALQ");

            entity.Property(e => e.Cliente)
                .HasMaxLength(201)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Marca)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Modelo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Placa)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PrecioTotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ReservaId).HasColumnName("ReservaID");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwVehiculosDisponible>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_VehiculosDisponibles", "ALQ");

            entity.Property(e => e.Color)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Combustible)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.Marca)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Modelo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Placa)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PrecioDiario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TipoVehiculo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Transmision)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.VehiculoId).HasColumnName("VehiculoID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
