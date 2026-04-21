using Microsoft.EntityFrameworkCore;
using Mapster;
using LaRicaNoche.Api.Data;
using LaRicaNoche.Api.Models;
using LaRicaNoche.Api.DTOs.Base;
using LaRicaNoche.Api.DTOs.Create;
using LaRicaNoche.Api.Services.Interfaces;
using LaRicaNoche.Api.Security;

namespace LaRicaNoche.Api.Services.Implementations;

public class UsuarioService : IUsuarioService
{
    private readonly LaRicaNocheDbContext _context;

    public UsuarioService(LaRicaNocheDbContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse<List<UsuarioResponseDto>>> GetAllAsync()
    {
        var usuarios = await _context.Usuarios.ToListAsync();
        return new BaseResponse<List<UsuarioResponseDto>> { Data = usuarios.Adapt<List<UsuarioResponseDto>>() };
    }

    public async Task<BaseResponse<UsuarioResponseDto>> GetByIdAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return new BaseResponse<UsuarioResponseDto> { IsSuccess = false, Message = "Usuario no encontrado" };

        return new BaseResponse<UsuarioResponseDto> { Data = usuario.Adapt<UsuarioResponseDto>() };
    }

    public async Task<BaseResponse<UsuarioResponseDto>> RegisterAsync(CreateUsuarioDto dto)
    {
        // 1. Verificar si el usuario ya existe
        if (await _context.Usuarios.AnyAsync(u => u.Username == dto.Username))
            return new BaseResponse<UsuarioResponseDto> { IsSuccess = false, Message = "El nombre de usuario ya está en uso." };

        // 2. Mapear y HASHEAR la contraseña
        var usuario = dto.Adapt<Usuario>();
        usuario.PasswordHash = PasswordHasher.HashPassword(dto.Password);

        // 3. Guardar en DB
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return new BaseResponse<UsuarioResponseDto> { Data = usuario.Adapt<UsuarioResponseDto>() };
    }

    public async Task<BaseResponse<UsuarioResponseDto>> LoginAsync(string username, string password)
    {
        // 1. Buscar al usuario
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == username);
        if (usuario == null) return new BaseResponse<UsuarioResponseDto> { IsSuccess = false, Message = "Credenciales incorrectas." };

        // 2. VERIFICAR la contraseña con el hash
        if (!PasswordHasher.VerifyPassword(password, usuario.PasswordHash))
            return new BaseResponse<UsuarioResponseDto> { IsSuccess = false, Message = "Credenciales incorrectas." };

        return new BaseResponse<UsuarioResponseDto> { Data = usuario.Adapt<UsuarioResponseDto>() };
    }
}
