using Microsoft.EntityFrameworkCore;
using Mapster;
using LaRicaNoche.Api.Data;
using LaRicaNoche.Api.Models;
using LaRicaNoche.Api.DTOs.Base;
using LaRicaNoche.Api.DTOs.Create;
using LaRicaNoche.Api.DTOs.Update;
using LaRicaNoche.Api.Services.Interfaces;
using LaRicaNoche.Api.Services.Interfaces.lua;

namespace LaRicaNoche.Api.Services.Implementations;

public class HabitacionService : IHabitacionService
{
    private readonly LaRicaNocheDbContext _context;
    private readonly ILuaService _luaService;

    public HabitacionService(LaRicaNocheDbContext context, ILuaService luaService)
    {
        _context = context;
        _luaService = luaService;
    }

    public async Task<BaseResponse<List<HabitacionResponseDto>>> GetAllAsync()
    {
        var entities = await _context.Habitaciones.ToListAsync();
        var response = entities.Select(h => new HabitacionResponseDto
        {
            IdHabitacion = h.IdHabitacion,
            NumeroHabitacion = h.NumeroHabitacion,
            Piso = h.Piso,
            PrecioNoche = h.PrecioNoche,
            Estado = h.Estado,
            FechaUltimoCheckout = h.FechaUltimoCheckout
        }).ToList();
        return new BaseResponse<List<HabitacionResponseDto>> { Data = response };
    }

    public async Task<BaseResponse<HabitacionResponseDto>> GetByIdAsync(int id)
    {
        var entity = await _context.Habitaciones.FindAsync(id);
        if (entity == null) return new BaseResponse<HabitacionResponseDto> { IsSuccess = false, Message = "Habitación no encontrada" };
        return new BaseResponse<HabitacionResponseDto> { Data = new HabitacionResponseDto
        {
            IdHabitacion = entity.IdHabitacion,
            NumeroHabitacion = entity.NumeroHabitacion,
            Piso = entity.Piso,
            PrecioNoche = entity.PrecioNoche,
            Estado = entity.Estado,
            FechaUltimoCheckout = entity.FechaUltimoCheckout
        } };
    }

    public async Task<BaseResponse<HabitacionResponseDto>> CreateAsync(CreateHabitacionDto dto)
    {
        // 1. LLAMADA AL CEREBRO DE LUA (Validamos solo el precio como acordamos)
        var luaResult = _luaService.ExecuteScriptFile("validar_precio.lua", dto.PrecioNoche);

        bool esValido = (bool)luaResult[0];
        string mensajeLua = luaResult[1].ToString()!;

        if (!esValido)
        {
            return new BaseResponse<HabitacionResponseDto> 
            { 
                IsSuccess = false, 
                Message = mensajeLua 
            };
        }

        // 2. GUARDADO EN SQL SERVER
        var entity = new Habitacion
        {
            NumeroHabitacion = dto.NumeroHabitacion,
            Piso = dto.Piso,
            PrecioNoche = dto.PrecioNoche,
            Estado = "Disponible"
        };
        _context.Habitaciones.Add(entity);
        await _context.SaveChangesAsync();

        return new BaseResponse<HabitacionResponseDto> { Data = new HabitacionResponseDto
        {
            IdHabitacion = entity.IdHabitacion,
            NumeroHabitacion = entity.NumeroHabitacion,
            Piso = entity.Piso,
            PrecioNoche = entity.PrecioNoche,
            Estado = entity.Estado
        } };
    }

    public async Task<BaseResponse<bool>> UpdateAsync(int id, UpdateHabitacionDto dto)
    {
        var entity = await _context.Habitaciones.FindAsync(id);
        if (entity == null) return new BaseResponse<bool> { IsSuccess = false, Message = "Habitación no encontrada" };
        
        dto.Adapt(entity);
        await _context.SaveChangesAsync();
        return new BaseResponse<bool> { Data = true };
    }

    public async Task<BaseResponse<bool>> MarcarLimpiezaCompletadaAsync(int id)
    {
        var entity = await _context.Habitaciones.FindAsync(id);
        if (entity == null) return new BaseResponse<bool> { IsSuccess = false, Message = "Habitación no encontrada" };

        if (entity.Estado != "Limpieza")
            return new BaseResponse<bool> { IsSuccess = false, Message = "La habitación no está en limpieza" };

        entity.Estado = "Disponible";
        await _context.SaveChangesAsync();

        return new BaseResponse<bool> { Data = true, Message = "Habitación marcada como limpia y disponible" };
    }
}
