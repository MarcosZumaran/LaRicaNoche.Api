using Microsoft.EntityFrameworkCore;
using Mapster;
using LaRicaNoche.Api.Data;
using LaRicaNoche.Api.Models;
using LaRicaNoche.Api.DTOs.Base;
using LaRicaNoche.Api.DTOs.Create;
using LaRicaNoche.Api.Services.Interfaces;
using LaRicaNoche.Api.Services.Interfaces.lua;

namespace LaRicaNoche.Api.Services.Implementations;

public class ReservaService : IReservaService
{
    private readonly LaRicaNocheDbContext _context;
    private readonly ILuaService _luaService;

    public ReservaService(LaRicaNocheDbContext context, ILuaService luaService)
    {
        _context = context;
        _luaService = luaService;
    }

    public async Task<BaseResponse<List<ReservaResponseDto>>> GetAllAsync()
    {
        var entities = await _context.Reservas
            .Include(r => r.Cliente)
            .Include(r => r.Habitacion)
            .ToListAsync();
        return new BaseResponse<List<ReservaResponseDto>> { Data = entities.Adapt<List<ReservaResponseDto>>() };
    }

    public async Task<BaseResponse<ReservaResponseDto>> GetByIdAsync(int id)
    {
        var entity = await _context.Reservas
            .Include(r => r.Cliente)
            .Include(r => r.Habitacion)
            .FirstOrDefaultAsync(r => r.IdReserva == id);
            
        if (entity == null) return new BaseResponse<ReservaResponseDto> { IsSuccess = false, Message = "Reserva no encontrada" };
        return new BaseResponse<ReservaResponseDto> { Data = entity.Adapt<ReservaResponseDto>() };
    }

    public async Task<BaseResponse<ReservaResponseDto>> CreateAsync(CreateReservaDto dto)
    {
        // 1. VERIFICAR HABITACIÓN (Criterio de Disponibilidad)
        var habitacion = await _context.Habitaciones.FindAsync(dto.IdHabitacion);
        if (habitacion == null) return new BaseResponse<ReservaResponseDto> { IsSuccess = false, Message = "Habitación inexistente." };
        if (habitacion.Estado != "Disponible") 
            return new BaseResponse<ReservaResponseDto> { IsSuccess = false, Message = "¡Habitación en estado: " + habitacion.Estado + "! No se puede reservar ahora." };

        // 2. VALIDACIÓN CON LUA
        var luaResult = _luaService.ExecuteScriptFile("validar_reserva.lua", dto.FechaEntrada, dto.FechaSalida, dto.MontoTotal);
        if (!(bool)luaResult[0]) 
            return new BaseResponse<ReservaResponseDto> { IsSuccess = false, Message = luaResult[1].ToString() };

        // 3. GUARDAR RESERVA
        var entity = dto.Adapt<Reserva>();
        _context.Reservas.Add(entity);
        await _context.SaveChangesAsync();

        // 4. MARCAR HABITACIÓN COMO OCUPADA (Lógica en C#)
        habitacion.Estado = "Ocupada";
        await _context.SaveChangesAsync();

        return new BaseResponse<ReservaResponseDto> { Data = entity.Adapt<ReservaResponseDto>() };
    }

    public async Task<BaseResponse<bool>> FinalizarReservaAsync(int id)
    {
        var entity = await _context.Reservas.FindAsync(id);
        if (entity == null) return new BaseResponse<bool> { IsSuccess = false, Message = "Reserva no encontrada." };
        
        // 1. CAMBIAR ESTADO DE RESERVA
        entity.EstadoReserva = "Finalizada";
        
        // 2. MARCAR HABITACIÓN COMO EN LIMPIEZA (Lógica en C#)
        var habitacion = await _context.Habitaciones.FindAsync(entity.IdHabitacion);
        if (habitacion != null)
        {
            habitacion.Estado = "Limpieza";
            habitacion.FechaUltimoCheckout = DateTime.Now;
        }
        
        await _context.SaveChangesAsync();
        return new BaseResponse<bool> { Data = true, Message = "Check-out realizado. Habitación enviada a limpieza." };
    }
}