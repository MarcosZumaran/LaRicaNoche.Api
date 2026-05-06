using Microsoft.EntityFrameworkCore;
using HotelGenericoApi.Data;
using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;
using HotelGenericoApi.Models;
using HotelGenericoApi.Services.Interfaces;
using NLua;

namespace HotelGenericoApi.Services.Implementations
{
    public class EstanciaService : IEstanciaService
    {
        private readonly HotelDbContext _db;
        private readonly ILuaService _lua;

        private const string ESTADO_ACTIVA = "Activa";
        private const string ESTADO_FINALIZADA = "Finalizada";
        private const string ESTADO_CONFIRMADA = "Confirmada";
        private const string ESTADO_CHECKIN_REALIZADO = "Check‑in realizado";

        public EstanciaService(HotelDbContext db, ILuaService lua)
        {
            _db = db;
            _lua = lua;
        }

        public async Task<IEnumerable<EstanciaResponseDto>> GetAllAsync()
        {
            return await _db.Estancias
                .Include(e => e.IdHabitacionNavigation)
                .Include(e => e.IdClienteTitularNavigation)
                .AsNoTracking()
                .Select(e => MapToResponse(e))
                .ToListAsync();
        }

        public async Task<EstanciaResponseDto?> GetByIdAsync(int id)
        {
            var e = await _db.Estancias
                .Include(e => e.IdHabitacionNavigation)
                .Include(e => e.IdClienteTitularNavigation)
                .FirstOrDefaultAsync(e => e.IdEstancia == id);

            return e is not null ? MapToResponse(e) : null;
        }

        public async Task<EstanciaResponseDto> CheckInAsync(CheckInDto dto, int? idUsuario)
        {
            // Verificar que la habitación esté disponible (permite_checkin = true)
            var habitacion = await _db.Habitaciones
                .Include(h => h.IdEstadoNavigation)
                .Include(h => h.IdTipoNavigation)
                .FirstOrDefaultAsync(h => h.IdHabitacion == dto.IdHabitacion);

            if (habitacion is null) throw new InvalidOperationException("La habitación no existe.");

            if (habitacion.IdEstadoNavigation == null || !habitacion.IdEstadoNavigation.PermiteCheckin) throw new InvalidOperationException("La habitación no está disponible para check‑in.");

            // 1.1 Validar reserva si se proporciona
            if (dto.IdReserva.HasValue)
            {
                var reserva = await _db.Reservas.FindAsync(dto.IdReserva.Value);
                if (reserva is null)
                    throw new InvalidOperationException("La reserva especificada no existe.");
                if (reserva.Estado != "Confirmada")
                    throw new InvalidOperationException("La reserva no está confirmada.");
                if (reserva.IdHabitacion != dto.IdHabitacion)
                    throw new InvalidOperationException("La habitación no coincide con la reserva.");
                // Actualizar estado de la reserva
                reserva.Estado = "Check‑in realizado";
            }

            Cliente cliente;
            if (dto.UsarClienteAnonimo)
            {
                cliente = await _db.Clientes
                    .FirstOrDefaultAsync(c => c.TipoDocumento == "0" && c.Documento == "00000000")
                    ?? throw new InvalidOperationException("Cliente anónimo no configurado en el sistema.");
            }
            else
            {
                cliente = await _db.Clientes.FirstOrDefaultAsync(c => c.TipoDocumento == dto.TipoDocumento && c.Documento == dto.Documento);

                if (cliente is null)
                {
                    cliente = new Cliente
                    {
                        TipoDocumento = dto.TipoDocumento,
                        Documento = dto.Documento,
                        Nombres = dto.Nombres,
                        Apellidos = dto.Apellidos,
                        Telefono = dto.Telefono,
                        FechaRegistro = DateTime.UtcNow
                    };
                    _db.Clientes.Add(cliente);
                    await _db.SaveChangesAsync();
                }
            }

            int noches = (int)(dto.FechaCheckoutPrevista.Date - DateTime.UtcNow.Date).TotalDays;
            if (noches < 1) noches = 1;

            var tarifa = await _db.Tarifas
                .Where(t => t.IdTipoHabitacion == habitacion.IdTipo && (t.FechaInicio == null || t.FechaInicio <= DateOnly.FromDateTime(DateTime.UtcNow)) && (t.FechaFin == null || t.FechaFin >= DateOnly.FromDateTime(DateTime.UtcNow)))
                .OrderByDescending(t => t.IdTemporadaNavigation != null ? t.IdTemporadaNavigation.Multiplier : 1)
                .FirstOrDefaultAsync();

            decimal precioNoche = tarifa?.Precio ?? habitacion.PrecioNoche;
            decimal montoSinIgv = precioNoche * noches;

            var configuracion = await _db.ConfiguracionHotels.FirstOrDefaultAsync();
            decimal tasaDefecto = configuracion?.TasaIgvHotel ?? 10.5m;
            decimal tasaIgv = tasaDefecto;
            decimal igvCalculado = montoSinIgv * (tasaIgv / 100);
            try
            {
                var resultado = _lua.CallFunction("hotel_tax_rules.lua", "Calculate_igv_hotel",
                    "10", montoSinIgv, "03");

                if (resultado.Length > 0 && resultado[0] is LuaTable tabla)
                {
                    tasaIgv = Convert.ToDecimal(tabla["tasa"]);
                    igvCalculado = Convert.ToDecimal(tabla["monto"]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al script Lua: " + ex.Message);
                throw new InvalidOperationException("Error al calcular el IGV: " + ex.Message);
            }

            decimal montoTotal = montoSinIgv + igvCalculado;

            var estancia = new Estancia
            {
                IdHabitacion = dto.IdHabitacion,
                IdClienteTitular = cliente.IdCliente,
                IdReserva = dto.IdReserva,
                FechaCheckin = DateTime.UtcNow,
                FechaCheckoutPrevista = dto.FechaCheckoutPrevista,
                MontoTotal = montoTotal,
                Estado = ESTADO_ACTIVA,
                CreatedAt = DateTime.UtcNow
            };
            _db.Estancias.Add(estancia);

            var estadoOcupada = await _db.CatEstadoHabitacions
                .FirstOrDefaultAsync(e => e.PermiteCheckout);
            if (estadoOcupada is not null)
            {
                int estadoAnterior = habitacion.IdEstado ?? 0;
                habitacion.IdEstado = estadoOcupada.IdEstado;
                habitacion.FechaUltimoCambio = DateTime.UtcNow;
                habitacion.UsuarioCambio = idUsuario;

                var historial = new HistorialEstadoHabitacion
                {
                    IdHabitacion = habitacion.IdHabitacion,
                    IdEstadoAnterior = estadoAnterior,
                    IdEstadoNuevo = estadoOcupada.IdEstado,
                    FechaCambio = DateTime.UtcNow,
                    IdUsuario = idUsuario,
                    Observacion = "Check‑In automático"
                };
                _db.HistorialEstadoHabitacions.Add(historial);
            }

            await _db.SaveChangesAsync();

            var comprobante = new Comprobante
            {
                IdEstancia = estancia.IdEstancia,
                TipoComprobante = "03",
                Serie = "B001",
                Correlativo = await ObtenerSiguienteCorrelativoAsync(),
                FechaEmision = DateTime.UtcNow,
                MontoTotal = montoTotal,
                IgvMonto = igvCalculado,
                ClienteDocumentoTipo = cliente.TipoDocumento,
                ClienteDocumentoNum = cliente.Documento,
                ClienteNombre = $"{cliente.Nombres} {cliente.Apellidos}",
                MetodoPago = dto.MetodoPago,
                IdEstadoSunat = 1
            };
            _db.Comprobantes.Add(comprobante);
            await _db.SaveChangesAsync();

            await _db.Entry(estancia).Reference(e => e.IdHabitacionNavigation).LoadAsync();
            await _db.Entry(estancia).Reference(e => e.IdClienteTitularNavigation).LoadAsync();

            return MapToResponse(estancia);
        }

        public async Task<EstanciaResponseDto> CheckOutAsync(int idEstancia, int? idUsuario)
        {
            var estancia = await _db.Estancias
                .Include(e => e.IdHabitacionNavigation)
                .FirstOrDefaultAsync(e => e.IdEstancia == idEstancia);

            if (estancia is null) throw new InvalidOperationException("La estancia no existe.");
            if (estancia.Estado != ESTADO_ACTIVA) throw new InvalidOperationException("La estancia no está activa.");

            estancia.Estado = ESTADO_FINALIZADA;
            estancia.FechaCheckoutReal = DateTime.UtcNow;

            var estadoDisponible = await _db.CatEstadoHabitacions
                .FirstOrDefaultAsync(e => e.PermiteCheckin);
            if (estancia.IdHabitacionNavigation is not null && estadoDisponible is not null)
            {
                var habitacion = estancia.IdHabitacionNavigation;
                int estadoAnterior = habitacion.IdEstado ?? 0;

                habitacion.IdEstado = estadoDisponible.IdEstado;
                habitacion.FechaUltimoCambio = DateTime.UtcNow;
                habitacion.UsuarioCambio = idUsuario;

                var historial = new HistorialEstadoHabitacion
                {
                    IdHabitacion = habitacion.IdHabitacion,
                    IdEstadoAnterior = estadoAnterior,
                    IdEstadoNuevo = estadoDisponible.IdEstado,
                    FechaCambio = DateTime.UtcNow,
                    IdUsuario = idUsuario,
                    Observacion = "Check‑Out automático"
                };
                _db.HistorialEstadoHabitacions.Add(historial);
            }

            await _db.SaveChangesAsync();

            await _db.Entry(estancia).Reference(e => e.IdHabitacionNavigation).LoadAsync();
            await _db.Entry(estancia).Reference(e => e.IdClienteTitularNavigation).LoadAsync();

            return MapToResponse(estancia);
        }

        private async Task<int> ObtenerSiguienteCorrelativoAsync()
        {
            int ultimo = await _db.Comprobantes
                .Where(c => c.Serie == "B001")
                .MaxAsync(c => (int?)c.Correlativo) ?? 0;
            return ultimo + 1;
        }

        private static EstanciaResponseDto MapToResponse(Estancia e)
        {
            return new EstanciaResponseDto(
                e.IdEstancia,
                e.IdHabitacion,
                e.IdHabitacionNavigation?.NumeroHabitacion,
                e.IdClienteTitular,
                e.IdClienteTitularNavigation is not null
                    ? $"{e.IdClienteTitularNavigation.Nombres} {e.IdClienteTitularNavigation.Apellidos}"
                    : null,
                e.FechaCheckin,
                e.FechaCheckoutPrevista,
                e.FechaCheckoutReal,
                e.MontoTotal,
                e.Estado,
                e.CreatedAt
            );
        }

        public async Task<EstanciaResponseDto> RegistrarConsumoAsync(int idEstancia, ConsumoEstanciaCreateDto dto, int? idUsuario)
        {
            var estancia = await _db.Estancias
                .Include(e => e.IdHabitacionNavigation)
                .Include(e => e.IdClienteTitularNavigation)
                .FirstOrDefaultAsync(e => e.IdEstancia == idEstancia);

            if (estancia is null) throw new InvalidOperationException("Estancia no encontrada.");
            if (estancia.Estado != ESTADO_ACTIVA) throw new InvalidOperationException("La estancia no está activa.");

            var producto = await _db.Productos.FindAsync(dto.IdProducto);
            if (producto is null) throw new InvalidOperationException("Producto no encontrado.");

            // Crear el item de consumo
            var itemEstancia = new ItemsEstancium
            {
                IdEstancia = idEstancia,
                IdProducto = dto.IdProducto,
                Cantidad = dto.Cantidad,
                PrecioUnitario = producto.PrecioUnitario,
                FechaRegistro = DateTime.UtcNow
            };
            _db.ItemsEstancia.Add(itemEstancia);

            // Actualizar el monto total de la estancia
            // El subtotal es calculado por la base de datos
            // Vamos a guardar y luego recargar para obtener el subtotal generado.
            await _db.SaveChangesAsync();

            // Recargamos el item para obtener el subtotal generado por la BD
            await _db.Entry(itemEstancia).ReloadAsync();
            decimal subtotal = itemEstancia.Subtotal ?? (producto.PrecioUnitario * dto.Cantidad);

            estancia.MontoTotal += subtotal;
            await _db.SaveChangesAsync();

            var comprobante = await _db.Comprobantes.FirstOrDefaultAsync(c => c.IdEstancia == estancia.IdEstancia);
            if (comprobante is not null)
            {
                comprobante.MontoTotal = estancia.MontoTotal;
                await _db.SaveChangesAsync();
            }

            return MapToResponse(estancia);
        }

        public async Task<ReservaResponseDto> CrearReservaAsync(ReservaCreateDto dto, int? idUsuario)
        {
            // Ejecutar toda la lógica dentro de una transacción serializable
            using var transaction = await _db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                // 1. Verificar que la habitación esté disponible para reserva
                var habitacion = await _db.Habitaciones
                    .Include(h => h.IdEstadoNavigation)
                    .Include(h => h.IdTipoNavigation)
                    .FirstOrDefaultAsync(h => h.IdHabitacion == dto.IdHabitacion);

                if (habitacion is null) throw new InvalidOperationException("La habitación no existe.");

                if (habitacion.IdEstadoNavigation == null || !habitacion.IdEstadoNavigation.PermiteCheckin)
                    throw new InvalidOperationException("La habitación no está disponible para reserva.");

                // 2. Validar solapamiento de fechas con otras reservas activas
                var conflicto = await _db.Reservas.AnyAsync(r =>
                    r.IdHabitacion == dto.IdHabitacion &&
                    r.Estado != "Cancelada" &&
                    r.FechaEntradaPrevista < dto.FechaSalidaPrevista &&
                    r.FechaSalidaPrevista > dto.FechaEntradaPrevista);

                if (conflicto)
                    throw new InvalidOperationException("La habitación ya está reservada en ese rango de fechas.");

                // 3. Obtener o crear cliente
                Cliente cliente;
                if (dto.UsarClienteAnonimo)
                {
                    cliente = await _db.Clientes
                        .FirstOrDefaultAsync(c => c.TipoDocumento == "0" && c.Documento == "00000000")
                        ?? throw new InvalidOperationException("Cliente anónimo no configurado en el sistema.");
                }
                else
                {
                    cliente = await _db.Clientes
                        .FirstOrDefaultAsync(c => c.TipoDocumento == dto.TipoDocumento && c.Documento == dto.Documento);

                    if (cliente is null)
                    {
                        cliente = new Cliente
                        {
                            TipoDocumento = dto.TipoDocumento,
                            Documento = dto.Documento,
                            Nombres = dto.Nombres,
                            Apellidos = dto.Apellidos,
                            Telefono = dto.Telefono,
                            FechaRegistro = DateTime.UtcNow
                        };
                        _db.Clientes.Add(cliente);
                        await _db.SaveChangesAsync();
                    }
                }

                // 4. Calcular noches y monto
                int noches = (int)(dto.FechaSalidaPrevista.Date - dto.FechaEntradaPrevista.Date).TotalDays;
                if (noches < 1) noches = 1;

                var tarifa = await _db.Tarifas
                    .Where(t => t.IdTipoHabitacion == habitacion.IdTipo &&
                        (t.FechaInicio == null || t.FechaInicio <= DateOnly.FromDateTime(dto.FechaEntradaPrevista)) &&
                        (t.FechaFin == null || t.FechaFin >= DateOnly.FromDateTime(dto.FechaEntradaPrevista)))
                    .OrderByDescending(t => t.IdTemporadaNavigation != null ? t.IdTemporadaNavigation.Multiplier : 1)
                    .FirstOrDefaultAsync();

                decimal precioNoche = tarifa?.Precio ?? habitacion.PrecioNoche;
                decimal montoSinIgv = precioNoche * noches;

                var configuracion = await _db.ConfiguracionHotels.FirstOrDefaultAsync();
                decimal tasaDefecto = configuracion?.TasaIgvHotel ?? 10.5m;
                decimal tasaIgv = tasaDefecto;
                decimal igvCalculado = montoSinIgv * (tasaIgv / 100);
                try
                {
                    var resultado = _lua.CallFunction("hotel_tax_rules.lua", "Calculate_igv_hotel",
                        "10", montoSinIgv, "03");

                    if (resultado.Length > 0 && resultado[0] is LuaTable tabla)
                    {
                        tasaIgv = Convert.ToDecimal(tabla["tasa"]);
                        igvCalculado = Convert.ToDecimal(tabla["monto"]);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al llamar al script Lua: " + ex.Message);
                    throw new InvalidOperationException("Error al calcular el IGV: " + ex.Message);
                }

                decimal montoTotal = montoSinIgv + igvCalculado;

                // 5. Crear la reserva
                var reserva = new Reserva
                {
                    IdHabitacion = dto.IdHabitacion,
                    IdCliente = cliente.IdCliente,
                    IdUsuario = idUsuario,
                    FechaRegistro = DateTime.UtcNow,
                    FechaEntradaPrevista = dto.FechaEntradaPrevista,
                    FechaSalidaPrevista = dto.FechaSalidaPrevista,
                    MontoTotal = montoTotal,
                    Estado = ESTADO_CONFIRMADA,
                    Observaciones = $"Reserva creada para {noches} noche(s)."
                };
                _db.Reservas.Add(reserva);

                // 6. Si la fecha de entrada es HOY, cambiar el estado de la habitación a Ocupada
                if (dto.FechaEntradaPrevista.Date == DateTime.UtcNow.Date)
                {
                    var estadoOcupada = await _db.CatEstadoHabitacions
                        .FirstOrDefaultAsync(e => e.PermiteCheckout);
                    if (estadoOcupada is not null)
                    {
                        int estadoAnterior = habitacion.IdEstado ?? 0;
                        habitacion.IdEstado = estadoOcupada.IdEstado;
                        habitacion.FechaUltimoCambio = DateTime.UtcNow;
                        habitacion.UsuarioCambio = idUsuario;

                        var historial = new HistorialEstadoHabitacion
                        {
                            IdHabitacion = habitacion.IdHabitacion,
                            IdEstadoAnterior = estadoAnterior,
                            IdEstadoNuevo = estadoOcupada.IdEstado,
                            FechaCambio = DateTime.UtcNow,
                            IdUsuario = idUsuario,
                            Observacion = "Reserva con entrada hoy — cambio automático a Ocupada"
                        };
                        _db.HistorialEstadoHabitacions.Add(historial);
                    }
                }

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ReservaResponseDto(
                    reserva.IdReserva,
                    reserva.IdHabitacion ?? 0,
                    habitacion.NumeroHabitacion,
                    $"{cliente.Nombres} {cliente.Apellidos}",
                    reserva.FechaEntradaPrevista,
                    reserva.FechaSalidaPrevista,
                    reserva.MontoTotal,
                    reserva.Estado ?? ESTADO_CONFIRMADA
                );
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ReservaResponseDto?> GetReservaByIdAsync(int id)
        {
            var reserva = await _db.Reservas
                .Include(r => r.IdHabitacionNavigation)
                .Include(r => r.IdClienteNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.IdReserva == id);

            if (reserva is null) return null;

            return new ReservaResponseDto(
                reserva.IdReserva,
                reserva.IdHabitacion ?? 0,
                reserva.IdHabitacionNavigation?.NumeroHabitacion,
                reserva.IdClienteNavigation is not null
                    ? $"{reserva.IdClienteNavigation.Nombres} {reserva.IdClienteNavigation.Apellidos}"
                    : null,
                reserva.FechaEntradaPrevista,
                reserva.FechaSalidaPrevista,
                reserva.MontoTotal,
                reserva.Estado ?? "Desconocido"
            );
        }
    }

}