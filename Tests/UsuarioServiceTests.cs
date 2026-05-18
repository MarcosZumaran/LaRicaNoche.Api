using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using HotelGenericoApi.Data;
using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.Mappings;
using HotelGenericoApi.Models;
using HotelGenericoApi.Services.Implementations;
using Xunit;

namespace HotelGenericoApi.Tests;

public class UsuarioServiceTests
{
    private static HotelDbContext CreateInMemoryDb(string dbName)
    {
        var options = new DbContextOptionsBuilder<HotelDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new HotelDbContext(options);
    }

    private static IConfiguration CreateJwtConfig()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "obTi6lRmI72jFEi+dLHYrKpETi5wm6Dy2H6qMvY0O3A=",
                ["Jwt:Issuer"] = "HotelGenericoApi",
                ["Jwt:Audience"] = "HotelGenericoApiClient"
            })!
            .Build();
    }

    [Fact]
    public async Task LoginAsync_CredencialesValidas_RetornaToken()
    {
        var db = CreateInMemoryDb("LoginValido");
        db.Usuarios.Add(new Usuario
        {
            Username = "test",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Pass123!"),
            IdRol = 1,
            EstaActivo = true,
            FechaCreacion = DateTime.UtcNow
        });
        db.RolesUsuario.Add(new RolUsuario { IdRol = 1, Nombre = "Administrador" });
        await db.SaveChangesAsync();

        var mapper = new UsuarioMapper();
        var config = CreateJwtConfig();
        var logger = new Mock<ILogger<UsuarioService>>().Object;

        var service = new UsuarioService(db, mapper, config, logger);
        var dto = new LoginDto { Username = "test", Password = "Pass123!" };

        var result = await service.LoginAsync(dto, "127.0.0.1", "TestAgent");

        Assert.NotNull(result);
        Assert.NotEmpty(result.Token);
        Assert.Equal("test", result.Usuario.Username);
    }

    [Fact]
    public async Task LoginAsync_CredencialesInvalidas_RetornaNull()
    {
        var db = CreateInMemoryDb("LoginInvalido");
        db.Usuarios.Add(new Usuario
        {
            Username = "test",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Pass123!"),
            IdRol = 1,
            EstaActivo = true,
            FechaCreacion = DateTime.UtcNow
        });
        await db.SaveChangesAsync();

        var mapper = new UsuarioMapper();
        var config = CreateJwtConfig();
        var logger = new Mock<ILogger<UsuarioService>>().Object;

        var service = new UsuarioService(db, mapper, config, logger);
        var dto = new LoginDto { Username = "test", Password = "WrongPassword" };

        var result = await service.LoginAsync(dto, "127.0.0.1", "TestAgent");

        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_UsuarioInactivo_RetornaNull()
    {
        var db = CreateInMemoryDb("LoginInactivo");
        db.Usuarios.Add(new Usuario
        {
            Username = "inactivo",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Pass123!"),
            IdRol = 1,
            EstaActivo = false,
            FechaCreacion = DateTime.UtcNow
        });
        await db.SaveChangesAsync();

        var mapper = new UsuarioMapper();
        var config = CreateJwtConfig();
        var logger = new Mock<ILogger<UsuarioService>>().Object;

        var service = new UsuarioService(db, mapper, config, logger);
        var dto = new LoginDto { Username = "inactivo", Password = "Pass123!" };

        var result = await service.LoginAsync(dto, "127.0.0.1", "TestAgent");

        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_Exitoso_RegistraLoginAttempt()
    {
        var db = CreateInMemoryDb("LoginAttemptOk");
        db.Usuarios.Add(new Usuario
        {
            Username = "test",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Pass123!"),
            IdRol = 1,
            EstaActivo = true,
            FechaCreacion = DateTime.UtcNow
        });
        db.RolesUsuario.Add(new RolUsuario { IdRol = 1, Nombre = "Administrador" });
        await db.SaveChangesAsync();

        var mapper = new UsuarioMapper();
        var config = CreateJwtConfig();
        var logger = new Mock<ILogger<UsuarioService>>().Object;

        var service = new UsuarioService(db, mapper, config, logger);
        var dto = new LoginDto { Username = "test", Password = "Pass123!" };

        await service.LoginAsync(dto, "127.0.0.1", "TestAgent");

        var attempts = await db.LoginAttempts.ToListAsync();
        Assert.Single(attempts);
        Assert.True(attempts[0].Succeeded);
        Assert.Equal("test", attempts[0].Username);
        Assert.Equal("127.0.0.1", attempts[0].IpAddress);
    }
}
