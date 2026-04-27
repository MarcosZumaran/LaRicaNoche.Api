namespace LaRicaNoche.Api.DTOs.Response;

public class TipoHabitacionResponseDto
{
    public int IdTipo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Capacidad { get; set; }
    public string? Descripcion { get; set; }
    public decimal PrecioBase { get; set; }
}