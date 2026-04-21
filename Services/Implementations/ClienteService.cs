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

public class ClienteService : IClienteService
{
    private readonly LaRicaNocheDbContext _context;
    private readonly ILuaService _luaService;

    public ClienteService(LaRicaNocheDbContext context, ILuaService luaService)
    {
        _context = context;
        _luaService = luaService;
    }

    public async Task<BaseResponse<List<ClienteResponseDto>>> GetAllAsync()
    {
        var entities = await _context.Clientes.ToListAsync();
        return new BaseResponse<List<ClienteResponseDto>> { Data = entities.Adapt<List<ClienteResponseDto>>() };
    }

    public async Task<BaseResponse<ClienteResponseDto>> GetByIdAsync(int id)
    {
        var entity = await _context.Clientes.FindAsync(id);
        if (entity == null) return new BaseResponse<ClienteResponseDto> { IsSuccess = false, Message = "Cliente no encontrado" };
        return new BaseResponse<ClienteResponseDto> { Data = entity.Adapt<ClienteResponseDto>() };
    }

    public async Task<BaseResponse<ClienteResponseDto>> GetByDocumentoAsync(string documento)
    {
        var entity = await _context.Clientes.FirstOrDefaultAsync(c => c.Documento == documento);
        if (entity == null) return new BaseResponse<ClienteResponseDto> { IsSuccess = false, Message = "Cliente no encontrado" };
        return new BaseResponse<ClienteResponseDto> { Data = entity.Adapt<ClienteResponseDto>() };
    }

    public async Task<BaseResponse<ClienteResponseDto>> CreateAsync(CreateClienteDto dto)
    {
        // --- VALIDACIÓN CON LUA ---
        var luaResult = _luaService.ExecuteScriptFile("validar_cliente.lua", dto.TipoDocumento, dto.Documento, dto.Nacionalidad);
        if (!(bool)luaResult[0]) 
            return new BaseResponse<ClienteResponseDto> { IsSuccess = false, Message = luaResult[1].ToString() };

        // Verificar si ya existe
        if (await _context.Clientes.AnyAsync(c => c.Documento == dto.Documento))
            return new BaseResponse<ClienteResponseDto> { IsSuccess = false, Message = "Este documento ya está registrado." };

        var entity = dto.Adapt<Cliente>();
        _context.Clientes.Add(entity);
        await _context.SaveChangesAsync();

        return new BaseResponse<ClienteResponseDto> { Data = entity.Adapt<ClienteResponseDto>() };
    }

    public async Task<BaseResponse<bool>> UpdateAsync(int id, UpdateClienteDto dto)
    {
        var entity = await _context.Clientes.FindAsync(id);
        if (entity == null) return new BaseResponse<bool> { IsSuccess = false, Message = "Cliente no encontrado" };

        entity.TipoDocumento = dto.TipoDocumento;
        entity.Documento = dto.Documento;
        entity.Nombres = dto.Nombres;
        entity.Apellidos = dto.Apellidos;
        entity.Telefono = dto.Telefono;
        entity.Email = dto.Email;
        entity.Nacionalidad = dto.Nacionalidad;
        entity.Direccion = dto.Direccion;

        await _context.SaveChangesAsync();
        return new BaseResponse<bool> { Data = true };
    }

    public async Task<BaseResponse<bool>> DeleteAsync(int id)
    {
        var entity = await _context.Clientes.FindAsync(id);
        if (entity == null) return new BaseResponse<bool> { IsSuccess = false, Message = "Cliente no encontrado" };

        _context.Clientes.Remove(entity);
        await _context.SaveChangesAsync();
        return new BaseResponse<bool> { Data = true };
    }
}
