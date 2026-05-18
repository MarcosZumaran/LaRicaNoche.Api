using HotelGenericoApi.Data;
using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.Models;
using HotelGenericoApi.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HotelGenericoApi.Services.Implementations;

public class SetupService
{
    private readonly HotelDbContext _db;
    private readonly ILogger<SetupService> _logger;

    public SetupService(HotelDbContext db, ILogger<SetupService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<bool> EsPrimerInicioAsync()
    {
        return !await _db.Usuarios.AnyAsync();
    }

    public async Task CrearUsuarioAdminAsync(UsuarioCreateDto dto)
    {
        if (!await EsPrimerInicioAsync())
            throw new BusinessRuleViolationException(BusinessErrorCode.SetupAlreadyDone, "El sistema ya fue inicializado.");

        var rolAdmin = await _db.RolesUsuario.FirstOrDefaultAsync(r => r.Nombre == "Administrador")
            ?? throw new BusinessRuleViolationException(BusinessErrorCode.ValidationError, "Rol Administrador no encontrado en el catálogo.");

        var usuario = new Usuario
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            IdRol = rolAdmin.IdRol,
            FechaCreacion = DateTime.UtcNow,
            EstaActivo = true
        };

        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync();
    }

    public async Task CrearUsuariosPorDefectoAsync()
    {
        // Asegurar que los roles existen
        var roles = new[] { "Administrador", "Recepcionista", "Limpieza" };
        foreach (var nombreRol in roles)
        {
            if (!await _db.RolesUsuario.AnyAsync(r => r.Nombre == nombreRol))
            {
                _db.RolesUsuario.Add(new RolUsuario { Nombre = nombreRol });
            }
        }
        await _db.SaveChangesAsync();

        // Obtener IDs de roles
        var rolAdmin = await _db.RolesUsuario.FirstAsync(r => r.Nombre == "Administrador");
        var rolRecepcion = await _db.RolesUsuario.FirstAsync(r => r.Nombre == "Recepcionista");
        var rolLimpieza = await _db.RolesUsuario.FirstAsync(r => r.Nombre == "Limpieza");

        // Definir usuarios por defecto
        var usuariosPorDefecto = new (string Username, int IdRol)[]
        {
        ("admin", rolAdmin.IdRol),
        ("recepcion", rolRecepcion.IdRol),
        ("limpieza", rolLimpieza.IdRol)
        };

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            foreach (var (username, idRol) in usuariosPorDefecto)
            {
                if (!await _db.Usuarios.AnyAsync(u => u.Username == username))
                {
                    var password = GenerarPasswordSeguro();
                    _db.Usuarios.Add(new Usuario
                    {
                        Username = username,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                        IdRol = idRol,
                        FechaCreacion = DateTime.UtcNow,
                        EstaActivo = true,
                        DebeCambiarPassword = true
                    });

                    _logger.LogWarning(
                        "Usuario creado: {Username}. Cambiar contraseña al iniciar sesión.",
                        username);
                }
            }

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private static string GenerarPasswordSeguro(int longitud = 16)
    {
        const string caracteresValidos = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789!@#$%&*()_-+=";
        var bytes = new byte[longitud];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return new string(bytes.Select(b => caracteresValidos[b % caracteresValidos.Length]).ToArray());
    }
}