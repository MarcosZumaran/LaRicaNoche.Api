namespace LaRicaNoche.Api.DTOs.Base;

public class DashboardDto
{
    public int TotalHabitaciones { get; set; }
    public int HabitacionesDisponibles { get; set; }
    public int HabitacionesOcupadas { get; set; }
    public int TotalReservasActivas { get; set; }
    public decimal IngresosHoy { get; set; }
    public decimal IngresosMes { get; set; }
    public int ClientesRegistrados { get; set; }
}

public class IngresosporFechaDto
{
    public DateTime Fecha { get; set; }
    public decimal IngresosReservas { get; set; }
    public decimal IngresosVentas { get; set; }
    public decimal Total { get; set; }
}

public class IngresosMensualDto
{
    public int Mes { get; set; }
    public string NombreMes { get; set; } = string.Empty;
    public decimal Total { get; set; }
}

public class ProductoVendidoDto
{
    public string NombreProducto { get; set; } = string.Empty;
    public int TotalVendido { get; set; }
    public decimal TotalIngresos { get; set; }
}

public class OcupacionHabitacionDto
{
    public int Piso { get; set; }
    public string Estado { get; set; } = string.Empty;
    public int Cantidad { get; set; }
}