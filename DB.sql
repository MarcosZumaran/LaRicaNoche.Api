USE [master];
GO

-- 1. CREACION DE LA BASE DE DATOS
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'LaRicaNocheDB')
BEGIN
    CREATE DATABASE [LaRicaNocheDB];
END
GO

USE [LaRicaNocheDB];
GO

-- 2. TABLAS

-- USUARIOS
CREATE TABLE usuarios (
    id_usuario INT PRIMARY KEY IDENTITY(1,1),
    username NVARCHAR(50) UNIQUE NOT NULL,
    password_hash NVARCHAR(MAX) NOT NULL,
    rol NVARCHAR(20) NOT NULL,
    fecha_creacion DATETIME DEFAULT GETDATE(),
    esta_activo BIT DEFAULT 1
);

-- CATEGORIAS
CREATE TABLE categorias (
    id_categoria INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(50) NOT NULL,
    descripcion NVARCHAR(120)
);

-- PRODUCTOS (Con Stock Mínimo para el dueño)
CREATE TABLE productos (
    id_producto INT PRIMARY KEY IDENTITY(1,1),
    id_categoria INT FOREIGN KEY REFERENCES categorias(id_categoria),
    nombre NVARCHAR(100) NOT NULL,
    precio_venta DECIMAL(10,2) NOT NULL,
    stock INT DEFAULT 0,
    stock_minimo INT DEFAULT 5, -- Alerta para el administrador
    unidad_medida VARCHAR(3) DEFAULT 'NIU', -- Código SUNAT para Unidades
    fecha_creacion DATETIME DEFAULT GETDATE()
);

-- CLIENTES (Con datos para TRH - MINCETUR)
CREATE TABLE clientes (
    id_cliente INT PRIMARY KEY IDENTITY(1,1),
    tipo_documento NVARCHAR(20) DEFAULT 'DNI',
    documento NVARCHAR(20) UNIQUE NOT NULL,
    nombres NVARCHAR(100),
    apellidos NVARCHAR(100),
    nacionalidad NVARCHAR(50) DEFAULT 'Peruana',
    fecha_nacimiento DATE,
    telefono NVARCHAR(15),
    email NVARCHAR(100),
    direccion NVARCHAR(200),
    fecha_registro DATETIME DEFAULT GETDATE()
);

-- HABITACIONES
CREATE TABLE habitaciones (
    id_habitacion INT PRIMARY KEY IDENTITY(1,1),
    numero_habitacion VARCHAR(10) UNIQUE NOT NULL,
    piso INT DEFAULT 1,
    precio_noche DECIMAL(10,2) DEFAULT 50.00,
    estado NVARCHAR(20) DEFAULT 'Disponible', -- 'Disponible', 'Ocupada', 'Limpieza', 'Mantenimiento'
    fecha_ultimo_checkout DATETIME NULL,
);

-- RESERVAS / ALQUILERES
CREATE TABLE reservas (
    id_reserva INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT FOREIGN KEY REFERENCES clientes(id_cliente),
    id_habitacion INT FOREIGN KEY REFERENCES habitaciones(id_habitacion),
    id_usuario_recepcion INT FOREIGN KEY REFERENCES usuarios(id_usuario),
    fecha_registro DATETIME DEFAULT GETDATE(),
    fecha_entrada DATETIME NOT NULL,
    fecha_salida DATETIME NOT NULL,
    monto_total DECIMAL(10,2) NOT NULL,
    metodo_pago NVARCHAR(20), -- 'Efectivo', 'Yape', 'Tarjeta'
    estado_reserva NVARCHAR(20) DEFAULT 'Activa',
    num_boleta NVARCHAR(20)    -- Número de boleta emitida
);

-- VENTAS DE PRODUCTOS
CREATE TABLE ventas (
    id_venta INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT NULL FOREIGN KEY REFERENCES clientes(id_cliente),
    id_usuario INT FOREIGN KEY REFERENCES usuarios(id_usuario),
    fecha_venta DATETIME DEFAULT GETDATE(),
    total_venta DECIMAL(10,2) NOT NULL,
    metodo_pago NVARCHAR(20) DEFAULT 'Efectivo'
);

-- DETALLE DE VENTAS
CREATE TABLE items_venta (
    id_item INT PRIMARY KEY IDENTITY(1,1),
    id_venta INT FOREIGN KEY REFERENCES ventas(id_venta),
    id_producto INT FOREIGN KEY REFERENCES productos(id_producto),
    cantidad INT NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    subtotal AS (cantidad * precio_unitario)
);

-- COMPROBANTES (BOLETAS/FACTURAS PARA SUNAT)
CREATE TABLE comprobantes (
    id_comprobante INT PRIMARY KEY IDENTITY(1,1),
    id_referencia INT NOT NULL, 
    tipo_referencia NVARCHAR(20) NOT NULL, -- 'Reserva' o 'Venta'
    tipo_comprobante NVARCHAR(20) DEFAULT 'Boleta',
    serie NVARCHAR(4) DEFAULT 'B001',
    correlativo INT NOT NULL,
    fecha_emision DATETIME DEFAULT GETDATE(),
    monto_total DECIMAL(10,2) NOT NULL,
    igv_monto AS (monto_total - (monto_total / 1.18)), -- Desglose automático para SUNAT
    cliente_documento NVARCHAR(20), -- DNI del cliente (SUNAT)
    cliente_nombres NVARCHAR(100),  -- Nombre completo (SUNAT)
    metodo_pago NVARCHAR(20),       -- Efectivo/Yape/Tarjeta
    estado_sunat NVARCHAR(20) DEFAULT 'Pendiente'
);

GO

-- 3. VISTA DE REPORTE DIARIO
CREATE OR ALTER VIEW v_cierre_caja_diario AS
SELECT 
    CAST(fecha_registro AS DATE) as Fecha,
    metodo_pago,
    SUM(monto_total) as Ingresos,
    'Reserva' as Concepto
FROM reservas
GROUP BY CAST(fecha_registro AS DATE), metodo_pago
UNION ALL
SELECT 
    CAST(fecha_venta AS DATE) as Fecha,
    metodo_pago,
    SUM(total_venta) as Ingresos,
    'VentaProducto' as Concepto
FROM ventas
GROUP BY CAST(fecha_venta AS DATE), metodo_pago;
GO
