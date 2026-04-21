using Microsoft.EntityFrameworkCore;
using LaRicaNoche.Api.Models;

namespace LaRicaNoche.Api.Data;

public class LaRicaNocheDbContext : DbContext
{
    public LaRicaNocheDbContext(DbContextOptions<LaRicaNocheDbContext> options) 
        : base(options) 
    { 
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Habitacion> Habitaciones { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<Venta> Ventas { get; set; }
    public DbSet<ItemVenta> ItemsVenta { get; set; }
    public DbSet<Comprobante> Comprobantes { get; set; }
    public DbSet<ReporteCajaDiario> ReporteCajaDiario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Solo configurar la vista - el resto por atributos [Column]
        modelBuilder.Entity<ReporteCajaDiario>(entity => 
        {
            entity.HasNoKey();
            entity.ToView("v_cierre_caja_diario");
        });
    }
}