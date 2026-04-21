USE [LaRicaNocheDB];
GO

-- 1. REPORTE: Ranking de productos más vendidos (Para saber qué comprar más)
CREATE OR ALTER PROCEDURE sp_RankingProductosVendidos
    @Top INT = 10
AS
BEGIN
    SELECT TOP (@Top)
        p.nombre,
        SUM(iv.cantidad) as TotalVendido,
        SUM(iv.subtotal) as TotalIngresos
    FROM productos p
    INNER JOIN items_venta iv ON p.id_producto = iv.id_producto
    GROUP BY p.nombre
    ORDER BY TotalVendido DESC;
END;
GO

-- 2. REPORTE: Ocupación por Piso (Para gestión de limpieza)
CREATE OR ALTER PROCEDURE sp_EstadoOcupacionPorPiso
AS
BEGIN
    SELECT 
        piso,
        estado,
        COUNT(*) as Cantidad
    FROM habitaciones
    GROUP BY piso, estado
    ORDER BY piso;
END;
GO

-- 3. ESTADISTICA: Ingresos Mensuales Comparativos
CREATE OR ALTER PROCEDURE sp_IngresosMensuales
    @Anio INT
AS
BEGIN
    SELECT 
        MONTH(Fecha) as Mes,
        SUM(Ingresos) as TotalMes
    FROM v_cierre_caja_diario
    WHERE YEAR(Fecha) = @Anio
    GROUP BY MONTH(Fecha)
    ORDER BY Mes;
END;
GO
