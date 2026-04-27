-- ================================================================
-- SCRIPT COMPLETO: Base de datos "La Rica Noche"
-- Versión final con tipos de habitación y cumplimiento SUNAT
-- Motor: SQL Server
-- ================================================================

USE [master];
GO

-- 1. CREACIÓN DE LA BASE DE DATOS
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'LaRicaNocheDB')
BEGIN
    CREATE DATABASE [LaRicaNocheDB];
END
GO

USE [LaRicaNocheDB];
GO

-- =============================================
-- TABLAS CATÁLOGO (SUNAT + NEGOCIO)
-- =============================================

-- Tipos de documento de identidad (catálogo SUNAT)
CREATE TABLE cat_tipo_documento (
    codigo CHAR(1) PRIMARY KEY,   -- '1' DNI, '6' RUC, '7' Pasaporte, etc.
    descripcion NVARCHAR(60) NOT NULL
);

-- Métodos de pago (catálogo SUNAT)
CREATE TABLE cat_metodo_pago (
    codigo CHAR(3) PRIMARY KEY,   -- '001' Depósito, '005' Efectivo, '006' Yape/Transferencia, '999' Otros
    descripcion NVARCHAR(60) NOT NULL
);

-- Tipos de comprobante SUNAT
CREATE TABLE cat_tipo_comprobante (
    codigo CHAR(2) PRIMARY KEY,   -- '03' Boleta, '01' Factura
    descripcion NVARCHAR(60) NOT NULL
);

-- Afectación al IGV (catálogo SUNAT)
CREATE TABLE cat_afectacion_igv (
    codigo CHAR(2) PRIMARY KEY,   -- '10' Gravado, '20' Exonerado
    descripcion NVARCHAR(60) NOT NULL
);

-- Estados de habitación
CREATE TABLE cat_estado_habitacion (
    id_estado INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(30) NOT NULL,       -- 'Disponible','Ocupada','Limpieza','Mantenimiento'
    descripcion NVARCHAR(100)
);

-- Roles de usuario
CREATE TABLE cat_rol_usuario (
    id_rol INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(30) NOT NULL        -- 'Administrador','Recepcionista','Limpieza'
);

-- =============================================
-- TABLAS PRINCIPALES
-- =============================================

-- USUARIOS
CREATE TABLE usuarios (
    id_usuario INT PRIMARY KEY IDENTITY(1,1),
    username NVARCHAR(50) UNIQUE NOT NULL,
    password_hash NVARCHAR(MAX) NOT NULL,
    id_rol INT FOREIGN KEY REFERENCES cat_rol_usuario(id_rol),
    fecha_creacion DATETIME DEFAULT GETDATE(),
    esta_activo BIT DEFAULT 1
);

-- CLIENTES
CREATE TABLE clientes (
    id_cliente INT PRIMARY KEY IDENTITY(1,1),
    tipo_documento CHAR(1) NOT NULL FOREIGN KEY REFERENCES cat_tipo_documento(codigo),
    documento NVARCHAR(20) NOT NULL,
    nombres NVARCHAR(100) NOT NULL,
    apellidos NVARCHAR(100) NOT NULL,
    nacionalidad NVARCHAR(50) DEFAULT 'PERUANA',
    fecha_nacimiento DATE,
    telefono NVARCHAR(15),
    email NVARCHAR(100),
    direccion NVARCHAR(200),
    fecha_registro DATETIME DEFAULT GETDATE(),
    CONSTRAINT UQ_cliente_documento UNIQUE (tipo_documento, documento)
);

-- TIPOS DE HABITACIÓN (nuevo)
CREATE TABLE tipos_habitacion (
    id_tipo INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(50) NOT NULL,           -- Ej: 'Matrimonial', 'Suite', 'Doble'
    capacidad INT DEFAULT 2,
    descripcion NVARCHAR(200),
    precio_base DECIMAL(10,2) DEFAULT 50.00 -- Precio sugerido, puede variar por habitación
);

-- HABITACIONES
CREATE TABLE habitaciones (
    id_habitacion INT PRIMARY KEY IDENTITY(1,1),
    numero_habitacion VARCHAR(10) UNIQUE NOT NULL,
    piso INT DEFAULT 1,
    descripcion NVARCHAR(200),
    id_tipo INT NOT NULL FOREIGN KEY REFERENCES tipos_habitacion(id_tipo),
    precio_noche DECIMAL(10,2) NOT NULL DEFAULT 50.00, -- Precio específico de esta habitación
    id_estado INT FOREIGN KEY REFERENCES cat_estado_habitacion(id_estado) DEFAULT 1, -- 1='Disponible'
    fecha_ultimo_cambio DATETIME DEFAULT GETDATE(),
    usuario_cambio INT FOREIGN KEY REFERENCES usuarios(id_usuario)
);

-- HISTÓRICO DE ESTADOS DE HABITACIÓN (auditoría)
CREATE TABLE historial_estado_habitacion (
    id_historial INT PRIMARY KEY IDENTITY(1,1),
    id_habitacion INT FOREIGN KEY REFERENCES habitaciones(id_habitacion),
    id_estado_anterior INT,
    id_estado_nuevo INT,
    fecha_cambio DATETIME DEFAULT GETDATE(),
    id_usuario INT FOREIGN KEY REFERENCES usuarios(id_usuario),
    observacion NVARCHAR(200)
);

-- RESERVAS (antes de la llegada)
CREATE TABLE reservas (
    id_reserva INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT FOREIGN KEY REFERENCES clientes(id_cliente),
    id_habitacion INT FOREIGN KEY REFERENCES habitaciones(id_habitacion),
    id_usuario INT FOREIGN KEY REFERENCES usuarios(id_usuario),
    fecha_registro DATETIME DEFAULT GETDATE(),
    fecha_entrada_prevista DATETIME NOT NULL,
    fecha_salida_prevista DATETIME NOT NULL,
    monto_total DECIMAL(10,2) NOT NULL,   -- calculado por API
    estado NVARCHAR(20) DEFAULT 'Pendiente', -- 'Pendiente','Confirmada','Cancelada','Check-in realizado'
    observaciones NVARCHAR(300)
);

-- ESTANCIAS (Check-in real)
CREATE TABLE estancias (
    id_estancia INT PRIMARY KEY IDENTITY(1,1),
    id_reserva INT NULL FOREIGN KEY REFERENCES reservas(id_reserva), -- nullable si walk-in
    id_habitacion INT FOREIGN KEY REFERENCES habitaciones(id_habitacion),
    id_cliente_titular INT FOREIGN KEY REFERENCES clientes(id_cliente),
    fecha_checkin DATETIME NOT NULL,
    fecha_checkout_prevista DATETIME NOT NULL,
    fecha_checkout_real DATETIME NULL,
    monto_total DECIMAL(10,2) NOT NULL,   -- calculado por API
    estado NVARCHAR(20) DEFAULT 'Activa', -- 'Activa','Finalizada','Anulada'
    created_at DATETIME DEFAULT GETDATE()
);

-- HUÉSPEDES (registro de personas que ocupan la habitación, obligatorio SUNAT y Migraciones)
CREATE TABLE huespedes (
    id_huesped INT PRIMARY KEY IDENTITY(1,1),
    id_estancia INT FOREIGN KEY REFERENCES estancias(id_estancia),
    id_cliente INT FOREIGN KEY REFERENCES clientes(id_cliente),
    es_titular BIT DEFAULT 0,
    fecha_registro DATETIME DEFAULT GETDATE()
);

-- PRODUCTOS / SERVICIOS ADICIONALES (aunque no los ofrecen ahora, queda preparado)
CREATE TABLE productos (
    id_producto INT PRIMARY KEY IDENTITY(1,1),
    codigo_sunat NVARCHAR(20) NULL,          -- Código de producto SUNAT (opcional)
    nombre NVARCHAR(100) NOT NULL,
    descripcion NVARCHAR(200),
    precio_unitario DECIMAL(10,2) NOT NULL,
    id_afectacion_igv CHAR(2) FOREIGN KEY REFERENCES cat_afectacion_igv(codigo) DEFAULT '10',  -- 10=Gravado
    stock INT DEFAULT 0,
    stock_minimo INT DEFAULT 5,
    unidad_medida NVARCHAR(3) DEFAULT 'NIU',
    created_at DATETIME DEFAULT GETDATE()
);

-- VENTAS DE PRODUCTOS (no vinculadas necesariamente a una estancia)
CREATE TABLE ventas (
    id_venta INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT NULL FOREIGN KEY REFERENCES clientes(id_cliente),
    id_usuario INT FOREIGN KEY REFERENCES usuarios(id_usuario),
    fecha_venta DATETIME DEFAULT GETDATE(),
    total DECIMAL(10,2) NOT NULL,
    metodo_pago CHAR(3) FOREIGN KEY REFERENCES cat_metodo_pago(codigo)
);

-- DETALLE VENTAS
CREATE TABLE items_venta (
    id_item INT PRIMARY KEY IDENTITY(1,1),
    id_venta INT FOREIGN KEY REFERENCES ventas(id_venta),
    id_producto INT FOREIGN KEY REFERENCES productos(id_producto),
    cantidad INT NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    subtotal AS (cantidad * precio_unitario) PERSISTED
);

-- COMPROBANTES ELECTRÓNICOS (SUNAT)
CREATE TABLE comprobantes (
    id_comprobante INT PRIMARY KEY IDENTITY(1,1),
    id_estancia INT NULL,                         -- si es factura/boleta de hospedaje
    id_venta INT NULL,                            -- si es boleta por venta de productos
    tipo_comprobante CHAR(2) FOREIGN KEY REFERENCES cat_tipo_comprobante(codigo),  -- '03' boleta, '01' factura
    serie NVARCHAR(4) NOT NULL,
    correlativo INT NOT NULL,
    fecha_emision DATETIME DEFAULT GETDATE(),
    monto_total DECIMAL(10,2) NOT NULL,
    igv_monto DECIMAL(10,2) NOT NULL,             -- calculado por API (10.5% de la base)
    cliente_documento_tipo CHAR(1) FOREIGN KEY REFERENCES cat_tipo_documento(codigo),
    cliente_documento_num NVARCHAR(20),
    cliente_nombre NVARCHAR(200),
    metodo_pago CHAR(3) FOREIGN KEY REFERENCES cat_metodo_pago(codigo),
    estado_sunat NVARCHAR(20) DEFAULT 'Pendiente', -- 'Pendiente','Aceptado','Rechazado'
    xml_firmado NVARCHAR(MAX),
    cdr_zip VARBINARY(MAX),
    UNIQUE (serie, correlativo)
);

-- =============================================
-- INSERCIÓN DE DATOS BASE (catálogos)
-- =============================================

INSERT INTO cat_tipo_documento (codigo, descripcion) VALUES
('1','DNI'),
('6','RUC'),
('7','Pasaporte'),
('0','Otros');

INSERT INTO cat_metodo_pago (codigo, descripcion) VALUES
('005','Efectivo'),
('006','Transferencia/Yape'),
('001','Depósito en cuenta'),
('999','Otros');

INSERT INTO cat_tipo_comprobante (codigo, descripcion) VALUES
('03','Boleta de Venta'),
('01','Factura');

INSERT INTO cat_afectacion_igv (codigo, descripcion) VALUES
('10','Gravado - Operación Onerosa'),
('20','Exonerado');

INSERT INTO cat_estado_habitacion (nombre, descripcion) VALUES
('Disponible', 'Lista para ser ocupada'),
('Ocupada', 'Con huéspedes actualmente'),
('Limpieza', 'En proceso de limpieza'),
('Mantenimiento', 'Fuera de servicio');

INSERT INTO cat_rol_usuario (nombre) VALUES
('Administrador'),
('Recepcionista'),
('Limpieza');

-- Tipo de habitación inicial (el que usan actualmente)
INSERT INTO tipos_habitacion (nombre, capacidad, descripcion, precio_base)
VALUES ('Matrimonial', 2, 'Habitación estándar para dos personas', 50.00);

-- =============================================
-- VISTAS ÚTILES
-- =============================================

-- Reporte diario de ingresos
CREATE OR ALTER VIEW v_cierre_caja_diario AS
SELECT
    CAST(e.fecha_checkin AS DATE) AS fecha,
    cm.descripcion AS metodo_pago,
    SUM(c.monto_total) AS ingresos,
    'Hospedaje' AS concepto
FROM comprobantes c
INNER JOIN estancias e ON c.id_estancia = e.id_estancia
INNER JOIN cat_metodo_pago cm ON c.metodo_pago = cm.codigo
WHERE c.tipo_comprobante = '03'   -- solo boletas, se puede ampliar
GROUP BY CAST(e.fecha_checkin AS DATE), cm.descripcion
UNION ALL
SELECT
    CAST(v.fecha_venta AS DATE) AS fecha,
    cm.descripcion AS metodo_pago,
    SUM(v.total) AS ingresos,
    'Productos' AS concepto
FROM ventas v
INNER JOIN cat_metodo_pago cm ON v.metodo_pago = cm.codigo
GROUP BY CAST(v.fecha_venta AS DATE), cm.descripcion;
GO

-- Vista de estado actual de habitaciones
CREATE OR ALTER VIEW v_estado_habitaciones AS
SELECT
    h.numero_habitacion,
    th.nombre AS tipo_habitacion,
    e.nombre AS estado,
    h.precio_noche,
    h.fecha_ultimo_cambio
FROM habitaciones h
INNER JOIN cat_estado_habitacion e ON h.id_estado = e.id_estado
INNER JOIN tipos_habitacion th ON h.id_tipo = th.id_tipo;
GO

PRINT 'Base de datos LaRicaNocheDB creada exitosamente.';
