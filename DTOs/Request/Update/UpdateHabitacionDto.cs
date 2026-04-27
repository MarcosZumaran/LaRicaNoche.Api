namespace LaRicaNoche.Api.DTOs.Request.Update;

public class UpdateHabitacionDto
{
    public string NumeroHabitacion { get; set; } = string.Empty;
    public int? Piso { get; set; }
    public string? Descripcion { get; set; }
    public decimal PrecioNoche { get; set; }
    public int IdTipo { get; set; }
    public int IdEstado { get; set; }
}