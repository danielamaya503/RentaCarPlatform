CREATE DATABASE RentaCarPlatform;
GO
USE RentaCarPlatform;
GO

-- ============================================================
--  SCHEMAS
-- ============================================================
CREATE SCHEMA SIS;
GO
CREATE SCHEMA ALQ;
GO

-- ============================================================
--  SIS.Roles
-- ============================================================
CREATE TABLE SIS.Roles (
    RolID       INT          IDENTITY(1,1) PRIMARY KEY,
    Nombre      VARCHAR(50)  NOT NULL UNIQUE,
    Descripcion VARCHAR(200) NULL,
    Activo      BIT          NOT NULL DEFAULT 1
);
GO

-- ============================================================
--  SIS.Usuarios
-- ============================================================
CREATE TABLE SIS.Usuarios (
    UsuarioID          INT          IDENTITY(1,1) PRIMARY KEY,
    RolID              INT          NOT NULL,
    NombreUsuario      VARCHAR(50)  NOT NULL UNIQUE,
    Email              VARCHAR(100) NOT NULL UNIQUE,
    Contrasena         VARCHAR(100) NOT NULL,
    Activo             BIT          NOT NULL DEFAULT 1,
    UltimoAcceso       DATETIME     NULL,
    CreadoPorUsuarioID INT          NULL,
    FechaCreacion      DATETIME     NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Usuarios_Rol       FOREIGN KEY (RolID)
        REFERENCES SIS.Roles(RolID),
    CONSTRAINT FK_Usuarios_CreadoPor FOREIGN KEY (CreadoPorUsuarioID)
        REFERENCES SIS.Usuarios(UsuarioID)
);
GO

-- ============================================================
--  ALQ.Marcas
-- ============================================================
CREATE TABLE ALQ.Marcas (
    MarcaID INT         IDENTITY(1,1) PRIMARY KEY,
    Nombre  VARCHAR(50) NOT NULL UNIQUE,
    Activo  BIT         NOT NULL DEFAULT 1
);
GO

-- ============================================================
--  ALQ.Modelos
-- ============================================================
CREATE TABLE ALQ.Modelos (
    ModeloID INT         IDENTITY(1,1) PRIMARY KEY,
    MarcaID  INT         NOT NULL,
    Nombre   VARCHAR(50) NOT NULL,
    Activo   BIT         NOT NULL DEFAULT 1,
    CONSTRAINT FK_Modelos_Marca       FOREIGN KEY (MarcaID)
        REFERENCES ALQ.Marcas(MarcaID),
    CONSTRAINT UQ_Modelos_MarcaModelo UNIQUE (MarcaID, Nombre)
);
GO

-- ============================================================
--  ALQ.TiposVehiculo
-- ============================================================
CREATE TABLE ALQ.TiposVehiculo (
    TipoVehiculoID INT         IDENTITY(1,1) PRIMARY KEY,
    Nombre         VARCHAR(50) NOT NULL UNIQUE,
    Activo         BIT         NOT NULL DEFAULT 1
);
GO

-- ============================================================
--  ALQ.EstadosVehiculo
-- ============================================================
CREATE TABLE ALQ.EstadosVehiculo (
    EstadoVehiculoID INT         IDENTITY(1,1) PRIMARY KEY,
    Nombre           VARCHAR(50) NOT NULL UNIQUE,
    ColorHex         VARCHAR(7)  NULL,
    Activo           BIT         NOT NULL DEFAULT 1
);
GO

-- ============================================================
--  ALQ.Caracteristicas
-- ============================================================
CREATE TABLE ALQ.Caracteristicas (
    CaracteristicaID INT          IDENTITY(1,1) PRIMARY KEY,
    Nombre           VARCHAR(100) NOT NULL UNIQUE,
    Icono            VARCHAR(50)  NULL,
    Activo           BIT          NOT NULL DEFAULT 1
);
GO

-- ============================================================
--  ALQ.Vehiculos
-- ============================================================
CREATE TABLE ALQ.Vehiculos (
    VehiculoID         INT           IDENTITY(1,1) PRIMARY KEY,
    ModeloID           INT           NOT NULL,
    TipoVehiculoID     INT           NOT NULL,
    EstadoVehiculoID   INT           NOT NULL,
    CreadoPorUsuarioID INT           NOT NULL,
    Anio               INT           NOT NULL,
    Placa              VARCHAR(20)   NOT NULL UNIQUE,
    Color              VARCHAR(30)   NOT NULL,
    Transmision        VARCHAR(20)   NOT NULL
        CONSTRAINT CHK_Vehiculos_Transmision
            CHECK (Transmision IN ('Automatico', 'Manual')),
    Combustible        VARCHAR(20)   NOT NULL
        CONSTRAINT CHK_Vehiculos_Combustible
            CHECK (Combustible IN ('Gasolina', 'Diesel', 'Hibrido', 'Electrico')),
    CapacidadPasajeros INT           NOT NULL,
    PrecioDiario       DECIMAL(10,2) NOT NULL,
    Descripcion        VARCHAR(MAX)  NULL,
    FechaCreacion      DATETIME      NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Vehiculos_Modelo     FOREIGN KEY (ModeloID)
        REFERENCES ALQ.Modelos(ModeloID),
    CONSTRAINT FK_Vehiculos_Tipo       FOREIGN KEY (TipoVehiculoID)
        REFERENCES ALQ.TiposVehiculo(TipoVehiculoID),
    CONSTRAINT FK_Vehiculos_Estado     FOREIGN KEY (EstadoVehiculoID)
        REFERENCES ALQ.EstadosVehiculo(EstadoVehiculoID),
    CONSTRAINT FK_Vehiculos_Usuario    FOREIGN KEY (CreadoPorUsuarioID)
        REFERENCES SIS.Usuarios(UsuarioID),
    CONSTRAINT CHK_Vehiculos_Anio      CHECK (Anio >= 1900 AND Anio <= 2100),
    CONSTRAINT CHK_Vehiculos_Precio    CHECK (PrecioDiario > 0),
    CONSTRAINT CHK_Vehiculos_Pasajeros CHECK (CapacidadPasajeros > 0)
);
GO

-- ============================================================
--  ALQ.VehiculoImagenes
-- ============================================================
CREATE TABLE ALQ.VehiculoImagenes (
    ImagenID      INT          IDENTITY(1,1) PRIMARY KEY,
    VehiculoID    INT          NOT NULL,
    URLImagen     VARCHAR(500) NOT NULL,
    EsPrincipal   BIT          NOT NULL DEFAULT 0,
    Orden         INT          NOT NULL DEFAULT 0,
    FechaCreacion DATETIME     NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_VehiculoImagenes_Vehiculo FOREIGN KEY (VehiculoID)
        REFERENCES ALQ.Vehiculos(VehiculoID) ON DELETE CASCADE
);
GO

-- ============================================================
--  ALQ.VehiculoCaracteristicas
-- ============================================================
CREATE TABLE ALQ.VehiculoCaracteristicas (
    VehiculoID       INT NOT NULL,
    CaracteristicaID INT NOT NULL,
    CONSTRAINT PK_VehiculoCaracteristicas PRIMARY KEY (VehiculoID, CaracteristicaID),
    CONSTRAINT FK_VehCar_Vehiculo FOREIGN KEY (VehiculoID)
        REFERENCES ALQ.Vehiculos(VehiculoID) ON DELETE CASCADE,
    CONSTRAINT FK_VehCar_Caracteristica FOREIGN KEY (CaracteristicaID)
        REFERENCES ALQ.Caracteristicas(CaracteristicaID)
);
GO

-- ============================================================
--  ALQ.DisponibilidadVehiculo
-- ============================================================
CREATE TABLE ALQ.DisponibilidadVehiculo (
    DisponibilidadID   INT          IDENTITY(1,1) PRIMARY KEY,
    VehiculoID         INT          NOT NULL,
    FechaInicio        DATE         NOT NULL,
    FechaFin           DATE         NOT NULL,
    Motivo             VARCHAR(100) NULL,
    CreadoPorUsuarioID INT          NOT NULL,
    FechaCreacion      DATETIME     NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Disponibilidad_Vehiculo FOREIGN KEY (VehiculoID)
        REFERENCES ALQ.Vehiculos(VehiculoID),
    CONSTRAINT FK_Disponibilidad_Usuario  FOREIGN KEY (CreadoPorUsuarioID)
        REFERENCES SIS.Usuarios(UsuarioID),
    CONSTRAINT CHK_Disponibilidad_Fechas  CHECK (FechaFin >= FechaInicio)
);
GO

-- ============================================================
--  ALQ.HistorialPrecios
-- ============================================================
CREATE TABLE ALQ.HistorialPrecios (
    HistorialPrecioID    INT           IDENTITY(1,1) PRIMARY KEY,
    VehiculoID           INT           NOT NULL,
    PrecioAnterior       DECIMAL(10,2) NOT NULL,
    PrecioNuevo          DECIMAL(10,2) NOT NULL,
    FechaCambio          DATETIME      NOT NULL DEFAULT GETDATE(),
    CambiadoPorUsuarioID INT           NULL,
    Motivo               VARCHAR(200)  NULL,
    CONSTRAINT FK_HistorialPrecios_Vehiculo FOREIGN KEY (VehiculoID)
        REFERENCES ALQ.Vehiculos(VehiculoID),
    CONSTRAINT FK_HistorialPrecios_Usuario  FOREIGN KEY (CambiadoPorUsuarioID)
        REFERENCES SIS.Usuarios(UsuarioID)
);
GO

-- ============================================================
--  ALQ.HistorialEstados
-- ============================================================
CREATE TABLE ALQ.HistorialEstados (
    HistorialEstadoID    INT          IDENTITY(1,1) PRIMARY KEY,
    VehiculoID           INT          NOT NULL,
    EstadoAnteriorID     INT          NULL,
    EstadoNuevoID        INT          NOT NULL,
    FechaCambio          DATETIME     NOT NULL DEFAULT GETDATE(),
    Motivo               VARCHAR(255) NULL,
    CambiadoPorUsuarioID INT          NULL,
    CONSTRAINT FK_HistorialEstados_Vehiculo    FOREIGN KEY (VehiculoID)
        REFERENCES ALQ.Vehiculos(VehiculoID),
    CONSTRAINT FK_HistorialEstados_EstAnterior FOREIGN KEY (EstadoAnteriorID)
        REFERENCES ALQ.EstadosVehiculo(EstadoVehiculoID),
    CONSTRAINT FK_HistorialEstados_EstNuevo    FOREIGN KEY (EstadoNuevoID)
        REFERENCES ALQ.EstadosVehiculo(EstadoVehiculoID),
    CONSTRAINT FK_HistorialEstados_Usuario     FOREIGN KEY (CambiadoPorUsuarioID)
        REFERENCES SIS.Usuarios(UsuarioID)
);
GO

-- ============================================================
--  ALQ.Clientes
-- ============================================================
CREATE TABLE ALQ.Clientes (
    ClienteID          INT          IDENTITY(1,1) PRIMARY KEY,
    Nombre             VARCHAR(100) NOT NULL,
    Apellido           VARCHAR(100) NOT NULL,
    DUI                VARCHAR(20)  NULL UNIQUE, 
    Telefono           VARCHAR(20)  NOT NULL,
    Email              VARCHAR(100) NULL UNIQUE,
    Direccion          VARCHAR(200) NULL,
    Activo             BIT          NOT NULL DEFAULT 1,
    CreadoPorUsuarioID INT          NOT NULL,
    FechaCreacion      DATETIME     NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Clientes_Usuario FOREIGN KEY (CreadoPorUsuarioID)
        REFERENCES SIS.Usuarios(UsuarioID)
);
GO

-- ============================================================
--  ALQ.Reservas
-- ============================================================
CREATE TABLE ALQ.Reservas (
    ReservaID          INT           IDENTITY(1,1) PRIMARY KEY,
    VehiculoID         INT           NOT NULL,
    ClienteID          INT           NOT NULL,     
    CreadoPorUsuarioID INT           NOT NULL,
    FechaInicio        DATE          NOT NULL,
    FechaFin           DATE          NOT NULL,
    Estado             VARCHAR(20)   NOT NULL DEFAULT 'Pendiente'
        CONSTRAINT CHK_Reservas_Estado
            CHECK (Estado IN ('Pendiente','Confirmada','En curso','Finalizada','Cancelada')),
    PrecioTotal        DECIMAL(10,2) NULL,
    MetodoPago         VARCHAR(50)   NULL,
    Notas              VARCHAR(MAX)  NULL,
    FechaCreacion      DATETIME      NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Reservas_Vehiculo FOREIGN KEY (VehiculoID)
        REFERENCES ALQ.Vehiculos(VehiculoID),
    CONSTRAINT FK_Reservas_Cliente  FOREIGN KEY (ClienteID)
        REFERENCES ALQ.Clientes(ClienteID),
    CONSTRAINT FK_Reservas_Usuario  FOREIGN KEY (CreadoPorUsuarioID)
        REFERENCES SIS.Usuarios(UsuarioID),
    CONSTRAINT CHK_Reservas_Fechas  CHECK (FechaFin >= FechaInicio)
);
GO

-- ============================================================
--  INDICES
-- ============================================================
CREATE INDEX IDX_Vehiculos_Estado      ON ALQ.Vehiculos(EstadoVehiculoID);
CREATE INDEX IDX_Vehiculos_Modelo      ON ALQ.Vehiculos(ModeloID);
CREATE INDEX IDX_Clientes_DUI          ON ALQ.Clientes(DUI);
CREATE INDEX IDX_Clientes_Email        ON ALQ.Clientes(Email);
CREATE INDEX IDX_Reservas_Vehiculo     ON ALQ.Reservas(VehiculoID);
CREATE INDEX IDX_Reservas_Cliente      ON ALQ.Reservas(ClienteID);
CREATE INDEX IDX_Reservas_Fechas       ON ALQ.Reservas(FechaInicio, FechaFin);
CREATE INDEX IDX_Reservas_Estado       ON ALQ.Reservas(Estado);
CREATE INDEX IDX_HistorialEstados_Veh  ON ALQ.HistorialEstados(VehiculoID);
CREATE INDEX IDX_HistorialPrecios_Veh  ON ALQ.HistorialPrecios(VehiculoID);
CREATE INDEX IDX_Disponibilidad_Veh    ON ALQ.DisponibilidadVehiculo(VehiculoID);
CREATE INDEX IDX_Disponibilidad_Fechas ON ALQ.DisponibilidadVehiculo(FechaInicio, FechaFin);
GO

-- ============================================================
--  TRIGGERS
-- ============================================================

-- Estado inicial al insertar un vehiculo
CREATE TRIGGER ALQ.trg_Vehiculos_Insert
ON ALQ.Vehiculos
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ALQ.HistorialEstados
        (VehiculoID, EstadoAnteriorID, EstadoNuevoID, FechaCambio, Motivo)
    SELECT
        i.VehiculoID, NULL, i.EstadoVehiculoID,
        GETDATE(), 'Estado inicial al registrar el vehiculo'
    FROM inserted i;
END;
GO

-- ------------------------------------------------------------
--  Update de vehiculo
--      - Actualiza FechaActualizacion
--      - Registra historial de estado si cambio
--      - Registra historial de precio si cambio
-- ------------------------------------------------------------
CREATE TRIGGER ALQ.trg_Vehiculos_Update
ON ALQ.Vehiculos
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;

    UPDATE v
    SET FechaActualizacion = GETDATE()
    FROM ALQ.Vehiculos v
    INNER JOIN inserted i ON v.VehiculoID = i.VehiculoID;

    IF UPDATE(EstadoVehiculoID)
    BEGIN
        INSERT INTO ALQ.HistorialEstados
            (VehiculoID, EstadoAnteriorID, EstadoNuevoID, FechaCambio, Motivo)
        SELECT i.VehiculoID, d.EstadoVehiculoID, i.EstadoVehiculoID,
               GETDATE(), 'Cambio de estado'
        FROM inserted i
        INNER JOIN deleted d ON i.VehiculoID = d.VehiculoID
        WHERE i.EstadoVehiculoID <> d.EstadoVehiculoID;
    END

    IF UPDATE(PrecioDiario)
    BEGIN
        INSERT INTO ALQ.HistorialPrecios
            (VehiculoID, PrecioAnterior, PrecioNuevo, FechaCambio, Motivo)
        SELECT i.VehiculoID, d.PrecioDiario, i.PrecioDiario,
               GETDATE(), 'Actualizacion de precio diario'
        FROM inserted i
        INNER JOIN deleted d ON i.VehiculoID = d.VehiculoID
        WHERE i.PrecioDiario <> d.PrecioDiario;
    END
END;
GO

-- ------------------------------------------------------------
--  Reservas INSERT/UPDATE
--      - Calcula PrecioTotal automaticamente
--      - Actualiza FechaActualizacion
-- ------------------------------------------------------------
CREATE TRIGGER ALQ.trg_Reservas_InsertUpdate
ON ALQ.Reservas
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;

    UPDATE r
    SET
        PrecioTotal        = v.PrecioDiario * (DATEDIFF(DAY, i.FechaInicio, i.FechaFin) + 1),
        FechaActualizacion = GETDATE()
    FROM ALQ.Reservas r
    INNER JOIN inserted i      ON r.ReservaID  = i.ReservaID
    INNER JOIN ALQ.Vehiculos v ON r.VehiculoID = v.VehiculoID;
END;
GO

-- ------------------------------------------------------------
--  Cambia estado del vehiculo segun estado de la reserva
--      Confirmada -> (sin cambio, reserva futura)
--      En curso   -> Rentado
--      Finalizada / Cancelada -> Disponible
-- ------------------------------------------------------------
CREATE TRIGGER ALQ.trg_Reservas_CambiarEstadoVehiculo
ON ALQ.Reservas
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(Estado)
    BEGIN
        UPDATE v
        SET EstadoVehiculoID = (
            SELECT EstadoVehiculoID FROM ALQ.EstadosVehiculo WHERE Nombre = 'Rentado'
        )
        FROM ALQ.Vehiculos v
        INNER JOIN inserted i ON v.VehiculoID = i.VehiculoID
        INNER JOIN deleted  d ON i.ReservaID  = d.ReservaID
        WHERE i.Estado = 'En curso'
          AND d.Estado <> 'En curso';

        UPDATE v
        SET EstadoVehiculoID = (
            SELECT EstadoVehiculoID FROM ALQ.EstadosVehiculo WHERE Nombre = 'Disponible'
        )
        FROM ALQ.Vehiculos v
        INNER JOIN inserted i ON v.VehiculoID = i.VehiculoID
        INNER JOIN deleted  d ON i.ReservaID  = d.ReservaID
        WHERE i.Estado IN ('Finalizada', 'Cancelada')
          AND d.Estado NOT IN ('Finalizada', 'Cancelada');
    END
END;
GO

-- ------------------------------------------------------------
-- Solo 1 imagen principal por vehiculo
-- ------------------------------------------------------------
CREATE TRIGGER ALQ.trg_VehiculoImagenes_UnaPrincipal
ON ALQ.VehiculoImagenes
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE(EsPrincipal)
    BEGIN
        UPDATE vi
        SET EsPrincipal = 0
        FROM ALQ.VehiculoImagenes vi
        INNER JOIN inserted i ON vi.VehiculoID = i.VehiculoID
        WHERE vi.ImagenID <> i.ImagenID
          AND i.EsPrincipal = 1;
    END
END;
GO

-- ------------------------------------------------------------
-- Bloqueo de reservas por vehiculo
--  Solo bloquea si la reserva nueva/modificada NO esta Cancelada.
--  Excluye reservas Canceladas/Finalizadas del chequeo.
-- ------------------------------------------------------------
CREATE TRIGGER ALQ.trg_Reservas_ValidarTraslape
ON ALQ.Reservas
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN ALQ.Reservas r
          ON  r.VehiculoID = i.VehiculoID
          AND r.ReservaID <> i.ReservaID
          AND r.Estado NOT IN ('Cancelada', 'Finalizada')
          AND i.Estado NOT IN ('Cancelada', 'Finalizada')
          AND r.FechaInicio <= i.FechaFin
          AND r.FechaFin   >= i.FechaInicio
    )
    BEGIN
        RAISERROR('El vehiculo ya tiene una reserva activa en ese rango de fechas.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END;
GO

-- ============================================================
--  VISTAS
-- ============================================================

-- Vehiculos disponibles con marca, modelo y tipo
CREATE VIEW ALQ.vw_VehiculosDisponibles AS
SELECT
    v.VehiculoID,
    ma.Nombre  AS Marca,
    mo.Nombre  AS Modelo,
    tv.Nombre  AS TipoVehiculo,
    v.Anio, v.Placa, v.Color,
    v.Transmision, v.Combustible,
    v.CapacidadPasajeros, v.PrecioDiario, v.Descripcion
FROM ALQ.Vehiculos       v
JOIN ALQ.Modelos         mo ON v.ModeloID         = mo.ModeloID
JOIN ALQ.Marcas          ma ON mo.MarcaID          = ma.MarcaID
JOIN ALQ.TiposVehiculo   tv ON v.TipoVehiculoID    = tv.TipoVehiculoID
JOIN ALQ.EstadosVehiculo ev ON v.EstadoVehiculoID  = ev.EstadoVehiculoID
WHERE ev.Nombre = 'Disponible';
GO

-- Reservas activas con datos del cliente y vehiculo
CREATE VIEW ALQ.vw_ReservasActivas AS
SELECT
    r.ReservaID,
    ma.Nombre  AS Marca,
    mo.Nombre  AS Modelo,
    v.Placa,
    c.Nombre + ' ' + c.Apellido AS Cliente,  -- NUEVO: datos del cliente desde ALQ.Clientes
    c.Telefono,
    c.Email,
    r.FechaInicio, r.FechaFin,
    r.PrecioTotal, r.Estado, r.MetodoPago
FROM ALQ.Reservas  r
JOIN ALQ.Vehiculos v  ON r.VehiculoID = v.VehiculoID
JOIN ALQ.Modelos   mo ON v.ModeloID   = mo.ModeloID
JOIN ALQ.Marcas    ma ON mo.MarcaID   = ma.MarcaID
JOIN ALQ.Clientes  c  ON r.ClienteID  = c.ClienteID  -- JOIN con tabla Clientes
WHERE r.Estado IN ('Confirmada', 'En curso');
GO

-- Historial de reservas por cliente (NUEVO v2)
CREATE VIEW ALQ.vw_HistorialReservasCliente AS
SELECT
    c.ClienteID,
    c.Nombre + ' ' + c.Apellido AS Cliente,
    c.DUI, c.Telefono, c.Email,
    ma.Nombre  AS Marca,
    mo.Nombre  AS Modelo,
    v.Placa,
    r.FechaInicio, r.FechaFin,
    DATEDIFF(DAY, r.FechaInicio, r.FechaFin) + 1 AS DiasRentados,
    r.PrecioTotal, r.Estado, r.MetodoPago,
    r.FechaCreacion AS FechaReserva
FROM ALQ.Reservas  r
JOIN ALQ.Clientes  c  ON r.ClienteID  = c.ClienteID
JOIN ALQ.Vehiculos v  ON r.VehiculoID = v.VehiculoID
JOIN ALQ.Modelos   mo ON v.ModeloID   = mo.ModeloID
JOIN ALQ.Marcas    ma ON mo.MarcaID   = ma.MarcaID;
GO

-- Historial completo de estados por vehiculo
CREATE VIEW ALQ.vw_HistorialEstados AS
SELECT
    v.VehiculoID,
    ma.Nombre  AS Marca,
    mo.Nombre  AS Modelo,
    v.Placa,
    ea.Nombre  AS EstadoAnterior,
    en.Nombre  AS EstadoNuevo,
    he.FechaCambio, he.Motivo,
    u.NombreUsuario AS CambiadoPor
FROM ALQ.HistorialEstados     he
JOIN ALQ.Vehiculos            v   ON he.VehiculoID           = v.VehiculoID
JOIN ALQ.Modelos              mo  ON v.ModeloID              = mo.ModeloID
JOIN ALQ.Marcas               ma  ON mo.MarcaID              = ma.MarcaID
JOIN ALQ.EstadosVehiculo      en  ON he.EstadoNuevoID        = en.EstadoVehiculoID
LEFT JOIN ALQ.EstadosVehiculo ea  ON he.EstadoAnteriorID     = ea.EstadoVehiculoID
LEFT JOIN SIS.Usuarios        u   ON he.CambiadoPorUsuarioID = u.UsuarioID;
GO

-- Historial de precios por vehiculo
CREATE VIEW ALQ.vw_HistorialPrecios AS
SELECT
    v.VehiculoID,
    ma.Nombre  AS Marca,
    mo.Nombre  AS Modelo,
    v.Placa,
    hp.PrecioAnterior, hp.PrecioNuevo,
    hp.FechaCambio, hp.Motivo,
    u.NombreUsuario AS CambiadoPor
FROM ALQ.HistorialPrecios hp
JOIN ALQ.Vehiculos  v    ON hp.VehiculoID            = v.VehiculoID
JOIN ALQ.Modelos    mo   ON v.ModeloID               = mo.ModeloID
JOIN ALQ.Marcas     ma   ON mo.MarcaID               = ma.MarcaID
LEFT JOIN SIS.Usuarios u ON hp.CambiadoPorUsuarioID  = u.UsuarioID;
GO

-- ============================================================
--  STORED PROCEDURES  (SIS)
-- ============================================================

CREATE PROCEDURE SIS.sp_CrearUsuario
    @RolID         INT,
    @NombreUsuario VARCHAR(50),
    @Email         VARCHAR(100),
    @Contrasena    VARCHAR(100),
    @CreadoPor     INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO SIS.Usuarios
            (RolID, NombreUsuario, Email, Contrasena, CreadoPorUsuarioID)
        VALUES (@RolID, @NombreUsuario, @Email, @Contrasena, @CreadoPor);
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE SIS.sp_ActualizarUsuario
    @UsuarioID  INT,
    @RolID      INT,
    @Contrasena VARCHAR(100) = NULL,
    @Activo     BIT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @Contrasena IS NOT NULL AND LEN(@Contrasena) > 0
            UPDATE SIS.Usuarios
            SET RolID = @RolID, Contrasena = @Contrasena, Activo = @Activo
            WHERE UsuarioID = @UsuarioID;
        ELSE
            UPDATE SIS.Usuarios
            SET RolID = @RolID, Activo = @Activo
            WHERE UsuarioID = @UsuarioID;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE SIS.sp_Login
    @NombreUsuario VARCHAR(50),
    @Contrasena    VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE SIS.Usuarios
    SET UltimoAcceso = GETDATE()
    WHERE NombreUsuario = @NombreUsuario
      AND Contrasena    = @Contrasena
      AND Activo        = 1;

    SELECT UsuarioID, NombreUsuario, Email, RolID, Activo
    FROM SIS.Usuarios
    WHERE NombreUsuario = @NombreUsuario
      AND Contrasena    = @Contrasena
      AND Activo        = 1;
END;
GO

-- ============================================================
--  STORED PROCEDURES  (ALQ)
-- ============================================================

-- ------------------------------------------------------------
--  SP: ALQ.sp_CrearReserva
--  Logica:
--    1. Si el cliente no existe por DUI/Email, lo crea automaticamente.
--    2. Valida que el vehiculo este Disponible.
--    3. Inserta la reserva (el trigger valida).
--    4. Retorna el ReservaID generado.
-- ------------------------------------------------------------
CREATE PROCEDURE ALQ.sp_CrearReserva
    @VehiculoID    INT,
    @CreadoPorID   INT,
    @Nombre        VARCHAR(100),
    @Apellido      VARCHAR(100),
    @DUI           VARCHAR(20)   = NULL,
    @Telefono      VARCHAR(20),
    @Email         VARCHAR(100)  = NULL,
    @Direccion     VARCHAR(200)  = NULL,
    @FechaInicio   DATE,
    @FechaFin      DATE,
    @MetodoPago    VARCHAR(50)   = NULL,
    @Notas         VARCHAR(MAX)  = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1. Buscar cliente existente (por DUI o Email)
        DECLARE @ClienteID INT;

        SELECT @ClienteID = ClienteID
        FROM ALQ.Clientes
        WHERE (@DUI   IS NOT NULL AND DUI   = @DUI)
           OR (@Email IS NOT NULL AND Email = @Email);

        -- Si no existe, crearlo
        IF @ClienteID IS NULL
        BEGIN
            INSERT INTO ALQ.Clientes
                (Nombre, Apellido, DUI, Telefono, Email, Direccion, CreadoPorUsuarioID)
            VALUES
                (@Nombre, @Apellido, @DUI, @Telefono, @Email, @Direccion, @CreadoPorID);

            SET @ClienteID = SCOPE_IDENTITY();
        END

        -- 2. Validar que el vehiculo este Disponible
        IF NOT EXISTS (
            SELECT 1
            FROM ALQ.Vehiculos v
            JOIN ALQ.EstadosVehiculo ev ON v.EstadoVehiculoID = ev.EstadoVehiculoID
            WHERE v.VehiculoID = @VehiculoID
              AND ev.Nombre    = 'Disponible'
        )
        BEGIN
            RAISERROR('El vehiculo no esta disponible para reserva.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- 3. Insertar la reserva
        --    El trigger trg_Reservas_ValidarTraslape protege.
        --    El trigger trg_Reservas_InsertUpdate calcula PrecioTotal automaticamente.
        INSERT INTO ALQ.Reservas
            (VehiculoID, ClienteID, CreadoPorUsuarioID,
             FechaInicio, FechaFin, Estado, MetodoPago, Notas)
        VALUES
            (@VehiculoID, @ClienteID, @CreadoPorID,
             @FechaInicio, @FechaFin, 'Pendiente', @MetodoPago, @Notas);

        -- 4. Retornar el ID generado
        SELECT SCOPE_IDENTITY() AS ReservaID, @ClienteID AS ClienteID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- ============================================================
--  DATOS SEMILLA  (catalogos base)
-- ============================================================

INSERT INTO SIS.Roles (Nombre, Descripcion) VALUES
('Administrador', 'Acceso total al sistema'),
('Empleado',      'Gestion de reservas y consulta de vehiculos'),
('Supervisor',    'Gestion de vehiculos y reportes');
GO

INSERT INTO ALQ.EstadosVehiculo (Nombre, ColorHex) VALUES
('Disponible',    '#28a745'),
('Rentado',       '#007bff'),
('Mantenimiento', '#ffc107'),
('Inactivo',      '#6c757d');
GO

INSERT INTO ALQ.TiposVehiculo (Nombre) VALUES
('Sedan'),('SUV'),('Pickup'),('Van'),
('Hatchback'),('Convertible'),('Camioneta');
GO

INSERT INTO ALQ.Marcas (Nombre) VALUES
('Toyota'),('Kia'),('Hyundai'),('Nissan'),
('Honda'),('Chevrolet'),('Ford'),('Mazda');
GO

INSERT INTO ALQ.Modelos (MarcaID, Nombre) VALUES
(1,'Corolla'),(1,'Hilux'),(1,'RAV4'),
(2,'Forte'),(2,'Sportage'),(2,'Picanto'),
(3,'Tucson'),(3,'Accent'),
(4,'Sentra'),(4,'Frontier'),
(5,'Civic'),(5,'CR-V'),
(6,'Sail'),(6,'Captiva'),
(7,'F-150'),(7,'Explorer'),
(8,'Mazda3'),(8,'CX-5');
GO

INSERT INTO ALQ.Caracteristicas (Nombre, Icono) VALUES
('Aire Acondicionado', 'fa-wind'),
('GPS',               'fa-map-marker-alt'),
('Bluetooth',         'fa-bluetooth'),
('Camara Trasera',    'fa-camera'),
('Techo Solar',       'fa-sun'),
('Asientos de Cuero', 'fa-chair'),
('USB',               'fa-usb'),
('Sensor de Parking', 'fa-parking');
GO

-- ============================================================
--  DATA QUEMADA
-- ============================================================

INSERT INTO SIS.Usuarios
    (RolID, NombreUsuario, Email, Contrasena, Activo, CreadoPorUsuarioID)
VALUES
(1, 'admin',      'admin@citycars.com',      'admin123', 1, NULL),
(2, 'empleado01', 'emp01@citycars.com',      'admin123', 1, 1),
(2, 'empleado02', 'emp02@citycars.com',      'admin123', 1, 1),
(3, 'supervisor', 'supervisor@citycars.com', 'admin123', 1, 1);
GO

INSERT INTO ALQ.Vehiculos
    (ModeloID, TipoVehiculoID, EstadoVehiculoID, CreadoPorUsuarioID,
     Anio, Placa, Color, Transmision, Combustible, CapacidadPasajeros, PrecioDiario, Descripcion)
VALUES
(1,  1, 1, 1, 2022, 'P-001-AAA', 'Blanco', 'Automatico', 'Gasolina', 5, 45.00, 'Toyota Corolla sedan, excelente estado, ideal para ciudad.'),
(3,  2, 1, 1, 2023, 'P-002-BBB', 'Negro',  'Automatico', 'Gasolina', 5, 70.00, 'Toyota RAV4 SUV espacioso, perfecto para viajes largos.'),
(2,  3, 2, 1, 2021, 'P-003-CCC', 'Plata',  'Manual',     'Diesel',   5, 80.00, 'Toyota Hilux pickup doble cabina, uso rudo.'),
(4,  1, 1, 1, 2022, 'P-004-DDD', 'Rojo',   'Automatico', 'Gasolina', 5, 40.00, 'Kia Forte economico y comodo para la familia.'),
(5,  2, 1, 1, 2023, 'P-005-EEE', 'Azul',   'Automatico', 'Gasolina', 5, 65.00, 'Kia Sportage, equipado con tecnologia de punta.'),
(7,  2, 3, 1, 2020, 'P-006-FFF', 'Gris',   'Automatico', 'Gasolina', 5, 60.00, 'Hyundai Tucson en mantenimiento preventivo.'),
(11, 1, 1, 1, 2023, 'P-007-GGG', 'Blanco', 'Automatico', 'Gasolina', 5, 50.00, 'Honda Civic, bajo consumo de combustible.'),
(18, 2, 1, 1, 2022, 'P-008-HHH', 'Rojo',   'Automatico', 'Gasolina', 5, 68.00, 'Mazda CX-5, diseno premium y manejo deportivo.'),
(9,  1, 4, 1, 2018, 'P-009-III', 'Beige',  'Manual',     'Gasolina', 5, 30.00, 'Nissan Sentra, temporalmente inactivo.'),
(15, 3, 1, 1, 2021, 'P-010-JJJ', 'Negro',  'Automatico', 'Gasolina', 5, 90.00, 'Ford F-150, pickup premium para carga liviana.'),
(6,  5, 1, 1, 2022, 'P-011-KKK', 'Verde',  'Manual',     'Gasolina', 5, 28.00, 'Kia Picanto, pequeno y agil para la ciudad.'),
(12, 2, 2, 1, 2023, 'P-012-LLL', 'Blanco', 'Automatico', 'Hibrido',  5, 75.00, 'Honda CR-V hibrido, actualmente rentado.');
GO

INSERT INTO ALQ.VehiculoImagenes (VehiculoID, URLImagen, EsPrincipal, Orden) VALUES
(1,  '/img/vehiculos/corolla_01.jpg',  1, 1),
(1,  '/img/vehiculos/corolla_02.jpg',  0, 2),
(1,  '/img/vehiculos/corolla_03.jpg',  0, 3),
(2,  '/img/vehiculos/rav4_01.jpg',     1, 1),
(2,  '/img/vehiculos/rav4_02.jpg',     0, 2),
(3,  '/img/vehiculos/hilux_01.jpg',    1, 1),
(3,  '/img/vehiculos/hilux_02.jpg',    0, 2),
(3,  '/img/vehiculos/hilux_03.jpg',    0, 3),
(4,  '/img/vehiculos/forte_01.jpg',    1, 1),
(4,  '/img/vehiculos/forte_02.jpg',    0, 2),
(5,  '/img/vehiculos/sportage_01.jpg', 1, 1),
(5,  '/img/vehiculos/sportage_02.jpg', 0, 2),
(7,  '/img/vehiculos/civic_01.jpg',    1, 1),
(7,  '/img/vehiculos/civic_02.jpg',    0, 2),
(8,  '/img/vehiculos/cx5_01.jpg',      1, 1),
(10, '/img/vehiculos/f150_01.jpg',     1, 1),
(10, '/img/vehiculos/f150_02.jpg',     0, 2),
(11, '/img/vehiculos/picanto_01.jpg',  1, 1),
(12, '/img/vehiculos/crv_01.jpg',      1, 1),
(12, '/img/vehiculos/crv_02.jpg',      0, 2);
GO

INSERT INTO ALQ.VehiculoCaracteristicas (VehiculoID, CaracteristicaID) VALUES
(1,1),(1,3),(1,7),
(2,1),(2,2),(2,3),(2,4),(2,5),(2,6),(2,7),(2,8),
(3,1),(3,3),(3,7),
(4,1),(4,3),(4,7),
(5,1),(5,2),(5,3),(5,4),(5,7),(5,8),
(7,1),(7,3),(7,4),(7,7),
(8,1),(8,2),(8,3),(8,4),(8,6),(8,7),(8,8),
(10,1),(10,2),(10,3),(10,4),(10,7),
(11,1),(11,3),
(12,1),(12,2),(12,3),(12,4),(12,5),(12,6),(12,7),(12,8);
GO

INSERT INTO ALQ.DisponibilidadVehiculo
    (VehiculoID, FechaInicio, FechaFin, Motivo, CreadoPorUsuarioID) VALUES
(2,  '2026-05-10', '2026-05-12', 'Evento corporativo reservado',  1),
(5,  '2026-05-20', '2026-05-22', 'Revision de frenos programada', 2),
(10, '2026-06-01', '2026-06-03', 'Mantenimiento preventivo',      1),
(8,  '2026-05-15', '2026-05-16', 'Limpieza y detallado',          3);
GO

-- NUEVO v2: Insertar clientes antes de las reservas
-- ClienteID: Mendoza=1, Lopez=2, Garcia=3, Perez=4, Hernandez=5,
--            Martinez=6, Ramirez=7, Soto=8, Aguilar=9, Cruz=10, Morales=11
INSERT INTO ALQ.Clientes
    (Nombre, Apellido, DUI, Telefono, Email, CreadoPorUsuarioID)
VALUES
('Carlos',   'Mendoza',  '00000001-1', '7701-1234', 'cmendoza@email.com',   2),
('Ana',      'Lopez',    '00000002-2', '7702-5678', 'alopez@email.com',     2),
('Roberto',  'Garcia',   '00000003-3', '7703-9012', 'rgarcia@email.com',    3),
('Maria',    'Perez',    '00000004-4', '7704-3456', 'mperez@email.com',     2),
('Jose',     'Hernandez','00000005-5', '7705-7890', 'jhernandez@email.com', 3),
('Laura',    'Martinez', '00000006-6', '7706-2345', 'lmartinez@email.com',  2),
('Diego',    'Ramirez',  '00000007-7', '7707-6789', 'dramirez@email.com',   3),
('Patricia', 'Soto',     '00000008-8', '7708-0123', 'psoto@email.com',      2),
('Fernando', 'Aguilar',  '00000009-9', '7709-4567', 'faguilar@email.com',   2),
('Valeria',  'Cruz',     '00000010-0', '7710-8901', 'vcruz@email.com',      3),
('Hugo',     'Morales',  '00000011-1', '7711-2345', 'hmorales@email.com',   2);
GO

-- NOTA: VehiculoID 3 y 12 estan en estado 'Rentado' (EstadoID=2),
--       por eso sus reservas se insertan con estado ya gestionado.
INSERT INTO ALQ.Reservas
    (VehiculoID, ClienteID, CreadoPorUsuarioID,
     FechaInicio, FechaFin, Estado, MetodoPago, Notas)
VALUES
(1,  1,  2, '2026-04-01','2026-04-04','Finalizada',  'Efectivo',      'Cliente frecuente'),
(4,  2,  2, '2026-04-05','2026-04-07','Finalizada',  'Tarjeta',        NULL),
(7,  3,  3, '2026-04-10','2026-04-12','Finalizada',  'Transferencia', 'Primera vez'),
(3,  4,  2, '2026-04-27','2026-05-01','En curso',    'Efectivo',      'Viaje al interior'),
(12, 5,  3, '2026-04-28','2026-04-30','En curso',    'Tarjeta',        NULL),
(2,  6,  2, '2026-05-02','2026-05-05','Confirmada',  'Tarjeta',       'Necesita silla para bebe'),
(5,  7,  3, '2026-05-03','2026-05-06','Confirmada',  'Transferencia',  NULL),
(8,  8,  2, '2026-05-08','2026-05-10','Confirmada',  'Efectivo',      'Requiere GPS activo'),
(11, 9,  2, '2026-05-15','2026-05-15','Pendiente',    NULL,             NULL),
(10, 10, 3, '2026-05-18','2026-05-20','Pendiente',    NULL,            'Consultar disponibilidad'),
(1,  11, 2, '2026-04-15','2026-04-17','Cancelada',    NULL,            'Cancelo por viaje');
GO
