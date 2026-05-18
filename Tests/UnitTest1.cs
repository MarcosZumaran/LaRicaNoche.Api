using Microsoft.Extensions.Logging;
using Moq;
using HotelGenericoApi.Data;
using HotelGenericoApi.Models;
using HotelGenericoApi.Services.Implementations;
using Xunit;

namespace HotelGenericoApi.Tests;

public class EstanciaServiceTests
{
    private HotelDbContext CreateContext() => TestDbContextFactory.Create();

    private static ILogger<T> CreateMockLogger<T>()
    {
        return new Mock<ILogger<T>>().Object;
    }

    [Fact]
    public async Task Create_EstanciaConHabitacionDisponible_CreaCorrectamente()
    {
        var db = CreateContext();
        var service = new EstanciaService(db, CreateMockLogger<EstanciaService>());

        var estancia = new Estancia
        {
            IdHabitacion = 1,
            IdClienteTitular = 1,
            FechaCheckin = DateTime.UtcNow,
            FechaCheckoutPrevista = DateTime.UtcNow.AddDays(2),
            Estado = "Activa"
        };

        var result = await service.CreateAsync(estancia);

        Assert.NotNull(result);
        Assert.Equal(1, result.IdHabitacion);
        Assert.Equal("Activa", result.Estado);

        var habitacion = await db.Habitaciones.FindAsync(1);
        Assert.NotNull(habitacion);
        Assert.Equal(2, habitacion.IdEstado); // Ocupada
    }

    [Fact]
    public async Task Checkout_EstanciaActiva_FinalizaCorrectamente()
    {
        var db = CreateContext();
        var service = new EstanciaService(db, CreateMockLogger<EstanciaService>());

        var estancia = new Estancia
        {
            IdHabitacion = 1,
            IdClienteTitular = 1,
            FechaCheckin = DateTime.UtcNow,
            FechaCheckoutPrevista = DateTime.UtcNow.AddDays(2),
            Estado = "Activa"
        };

        var creada = await service.CreateAsync(estancia);

        var result = await service.CheckoutAsync(creada.IdEstancia, 1);
        Assert.NotNull(result);
        Assert.Equal("Finalizada", result.Estado);
        Assert.NotNull(result.FechaCheckoutReal);
    }

    [Fact]
    public async Task AddHuesped_EstanciaExistente_AgregaCorrectamente()
    {
        var db = CreateContext();
        var service = new EstanciaService(db, CreateMockLogger<EstanciaService>());

        var estancia = new Estancia
        {
            IdHabitacion = 1,
            IdClienteTitular = 1,
            FechaCheckin = DateTime.UtcNow,
            FechaCheckoutPrevista = DateTime.UtcNow.AddDays(2),
            Estado = "Activa"
        };

        var creada = await service.CreateAsync(estancia);

        var huesped = new Huesped
        {
            IdCliente = 2,
            EsTitular = false
        };

        var result = await service.AddHuespedAsync(creada.IdEstancia, huesped);
        Assert.True(result);

        var dbHuesped = await db.Huespedes.FindAsync(huesped.IdHuesped);
        Assert.NotNull(dbHuesped);
        Assert.Equal(creada.IdEstancia, dbHuesped.IdEstancia);
    }

    [Fact]
    public async Task AddConsumo_EstanciaExistente_AgregaCorrectamente()
    {
        var db = CreateContext();
        var service = new EstanciaService(db, CreateMockLogger<EstanciaService>());

        var estancia = new Estancia
        {
            IdHabitacion = 1,
            IdClienteTitular = 1,
            FechaCheckin = DateTime.UtcNow,
            FechaCheckoutPrevista = DateTime.UtcNow.AddDays(2),
            Estado = "Activa"
        };

        var creada = await service.CreateAsync(estancia);

        var item = new ItemEstancia
        {
            IdProducto = 1,
            Cantidad = 2,
            PrecioUnitario = 10.5m
        };

        var result = await service.AddConsumoAsync(creada.IdEstancia, item);
        Assert.True(result);
    }

    [Fact]
    public async Task Transicion_Disponible_Ocupada_EsValida()
    {
        var db = TestDbContextFactory.Create();
        var validador = new ValidadorEstadoService(db);
        Assert.True(await validador.EsTransicionValidaAsync(1, 2));
    }

    [Fact]
    public async Task Transicion_Mantenimiento_Ocupada_NoPermitida()
    {
        var db = TestDbContextFactory.Create();
        var validador = new ValidadorEstadoService(db);
        Assert.False(await validador.EsTransicionValidaAsync(4, 2));
    }
}
