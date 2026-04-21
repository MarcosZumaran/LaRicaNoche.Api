using Microsoft.EntityFrameworkCore;
using LaRicaNoche.Api.Data;
using LaRicaNoche.Api.DTOs.Base;
using LaRicaNoche.Api.Services.Interfaces;
using LaRicaNoche.Api.Services.Interfaces.lua;

namespace LaRicaNoche.Api.Services.Implementations;

public class ReporteService : IReporteService
{
    private readonly LaRicaNocheDbContext _context;
    private readonly ILuaService _luaService;

    public ReporteService(LaRicaNocheDbContext context, ILuaService luaService)
    {
        _context = context;
        _luaService = luaService;
    }

    public async Task<BaseResponse<DashboardDto>> GetDashboardAsync()
    {
        var hoy = DateTime.Today;
        var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);

        var totalHab = await _context.Habitaciones.CountAsync();
        var habDisp = await _context.Habitaciones.CountAsync(h => h.Estado == "Disponible");
        var habOcup = await _context.Habitaciones.CountAsync(h => h.Estado == "Ocupada");
        var reservasActivas = await _context.Reservas.CountAsync(r => r.EstadoReserva == "Activa");
        var clientesReg = await _context.Clientes.CountAsync();

        var ingresosReservasHoy = await _context.Reservas
            .Where(r => r.FechaRegistro.Date == hoy)
            .SumAsync(r => r.MontoTotal);

        var ingresosVentasHoy = await _context.Ventas
            .Where(v => v.FechaVenta.Date == hoy)
            .SumAsync(v => v.TotalVenta);

        var ingresosReservasMes = await _context.Reservas
            .Where(r => r.FechaRegistro.Date >= inicioMes)
            .SumAsync(r => r.MontoTotal);

        var ingresosVentasMes = await _context.Ventas
            .Where(v => v.FechaVenta.Date >= inicioMes)
            .SumAsync(v => v.TotalVenta);

        var response = new DashboardDto
        {
            TotalHabitaciones = totalHab,
            HabitacionesDisponibles = habDisp,
            HabitacionesOcupadas = habOcup,
            TotalReservasActivas = reservasActivas,
            IngresosHoy = ingresosReservasHoy + ingresosVentasHoy,
            IngresosMes = ingresosReservasMes + ingresosVentasMes,
            ClientesRegistrados = clientesReg
        };

        return new BaseResponse<DashboardDto> { Data = response };
    }

    public async Task<BaseResponse<List<IngresosporFechaDto>>> GetIngresosPorRangoAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        // VALIDACIÓN CON LUA - Rango máximo 90 días
        var luaResult = _luaService.ExecuteScriptFile("validar_fechas_reporte.lua", fechaInicio, fechaFin);
        if (!(bool)luaResult[0]) 
            return new BaseResponse<List<IngresosporFechaDto>> { IsSuccess = false, Message = luaResult[1].ToString() };

        var reservas = await _context.Reservas
            .Where(r => r.FechaRegistro.Date >= fechaInicio.Date && r.FechaRegistro.Date <= fechaFin.Date)
            .GroupBy(r => r.FechaRegistro.Date)
            .Select(g => new IngresosporFechaDto
            {
                Fecha = g.Key,
                IngresosReservas = g.Sum(r => r.MontoTotal),
                IngresosVentas = 0,
                Total = g.Sum(r => r.MontoTotal)
            })
            .ToListAsync();

        var ventas = await _context.Ventas
            .Where(v => v.FechaVenta.Date >= fechaInicio.Date && v.FechaVenta.Date <= fechaFin.Date)
            .GroupBy(v => v.FechaVenta.Date)
            .Select(g => new IngresosporFechaDto
            {
                Fecha = g.Key,
                IngresosReservas = 0,
                IngresosVentas = g.Sum(v => v.TotalVenta),
                Total = g.Sum(v => v.TotalVenta)
            })
            .ToListAsync();

        var fechas = new List<IngresosporFechaDto>();
        for (var fecha = fechaInicio.Date; fecha <= fechaFin.Date; fecha = fecha.AddDays(1))
        {
            var res = reservas.FirstOrDefault(r => r.Fecha.Date == fecha);
            var vent = ventas.FirstOrDefault(v => v.Fecha.Date == fecha);

            fechas.Add(new IngresosporFechaDto
            {
                Fecha = fecha,
                IngresosReservas = res?.IngresosReservas ?? 0,
                IngresosVentas = vent?.IngresosVentas ?? 0,
                Total = (res?.Total ?? 0) + (vent?.Total ?? 0)
            });
        }

        return new BaseResponse<List<IngresosporFechaDto>> { Data = fechas };
    }

    public async Task<BaseResponse<List<IngresosMensualDto>>> GetIngresosMensualesAsync(int anio)
    {
        var meses = new[] { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };

        var reservas = await _context.Reservas
            .Where(r => r.FechaRegistro.Year == anio)
            .GroupBy(r => r.FechaRegistro.Month)
            .Select(g => new { Mes = g.Key, Total = g.Sum(r => r.MontoTotal) })
            .ToListAsync();

        var ventas = await _context.Ventas
            .Where(v => v.FechaVenta.Year == anio)
            .GroupBy(v => v.FechaVenta.Month)
            .Select(g => new { Mes = g.Key, Total = g.Sum(v => v.TotalVenta) })
            .ToListAsync();

        var resultado = new List<IngresosMensualDto>();
        for (int mes = 1; mes <= 12; mes++)
        {
            var totalRes = reservas.FirstOrDefault(r => r.Mes == mes)?.Total ?? 0;
            var totalVent = ventas.FirstOrDefault(v => v.Mes == mes)?.Total ?? 0;

            resultado.Add(new IngresosMensualDto
            {
                Mes = mes,
                NombreMes = meses[mes - 1],
                Total = totalRes + totalVent
            });
        }

        return new BaseResponse<List<IngresosMensualDto>> { Data = resultado };
    }

    public async Task<BaseResponse<List<ProductoVendidoDto>>> GetProductosMasVendidosAsync(int top = 10)
    {
        var productos = await _context.ItemsVenta
            .Include(i => i.Producto)
            .GroupBy(i => i.Producto!.Nombre)
            .Select(g => new ProductoVendidoDto
            {
                NombreProducto = g.Key,
                TotalVendido = g.Sum(i => i.Cantidad),
                TotalIngresos = g.Sum(i => i.Cantidad * i.PrecioUnitario)
            })
            .OrderByDescending(p => p.TotalVendido)
            .Take(top)
            .ToListAsync();

        return new BaseResponse<List<ProductoVendidoDto>> { Data = productos };
    }

    public async Task<BaseResponse<List<OcupacionHabitacionDto>>> GetOcupacionPorPisoAsync()
    {
        var ocupacion = await _context.Habitaciones
            .GroupBy(h => new { h.Piso, h.Estado })
            .Select(g => new OcupacionHabitacionDto
            {
                Piso = g.Key.Piso,
                Estado = g.Key.Estado,
                Cantidad = g.Count()
            })
            .OrderBy(h => h.Piso)
            .ThenBy(h => h.Estado)
            .ToListAsync();

        return new BaseResponse<List<OcupacionHabitacionDto>> { Data = ocupacion };
    }

    public async Task<BaseResponse<List<DTOs.Base.ProductoResponseDto>>> GetAlertasStockAsync()
    {
        var productos = await _context.Productos
            .Include(p => p.Categoria)
            .Where(p => p.Stock <= p.StockMinimo)
            .ToListAsync();

        var response = productos.Select(p => new DTOs.Base.ProductoResponseDto
        {
            IdProducto = p.IdProducto,
            IdCategoria = p.IdCategoria,
            NombreCategoria = p.Categoria?.Nombre,
            Nombre = p.Nombre,
            PrecioVenta = p.PrecioVenta,
            Stock = p.Stock,
            StockMinimo = p.StockMinimo,
            UnidadMedida = p.UnidadMedida,
            TieneStock = p.Stock > 0
        }).ToList();

        return new BaseResponse<List<DTOs.Base.ProductoResponseDto>> { Data = response };
    }

    public async Task<BaseResponse<List<ReporteCajaDto>>> GetCierreCajaAsync(DateTime? fechaInicio, DateTime? fechaFin)
    {
        var query = _context.ReporteCajaDiario.AsQueryable();

        if (fechaInicio.HasValue)
            query = query.Where(r => r.Fecha >= fechaInicio.Value);

        if (fechaFin.HasValue)
            query = query.Where(r => r.Fecha <= fechaFin.Value);

        var datos = await query.OrderByDescending(r => r.Fecha).ToListAsync();

        var response = datos.Select(r => new ReporteCajaDto
        {
            Fecha = r.Fecha,
            MetodoPago = r.MetodoPago ?? "",
            Ingresos = r.Ingresos,
            Concepto = r.Concepto
        }).ToList();

        return new BaseResponse<List<ReporteCajaDto>> { Data = response };
    }

    public async Task<BaseResponse<List<ResumenCajaDto>>> GetResumenCajaAsync(DateTime? fechaInicio, DateTime? fechaFin)
    {
        var query = _context.ReporteCajaDiario.AsQueryable();

        if (fechaInicio.HasValue)
            query = query.Where(r => r.Fecha >= fechaInicio.Value);

        if (fechaFin.HasValue)
            query = query.Where(r => r.Fecha <= fechaFin.Value);

        var datos = await query.ToListAsync();

        var resumen = datos
            .GroupBy(r => r.Fecha.Date)
            .Select(g => new ResumenCajaDto
            {
                Fecha = g.Key,
                Efectivo = g.Where(r => r.MetodoPago == "Efectivo").Sum(r => r.Ingresos),
                Yape = g.Where(r => r.MetodoPago == "Yape").Sum(r => r.Ingresos),
                Tarjeta = g.Where(r => r.MetodoPago == "Tarjeta").Sum(r => r.Ingresos),
                Total = g.Sum(r => r.Ingresos)
            })
            .OrderByDescending(r => r.Fecha)
            .ToList();

        return new BaseResponse<List<ResumenCajaDto>> { Data = resumen };
    }
}