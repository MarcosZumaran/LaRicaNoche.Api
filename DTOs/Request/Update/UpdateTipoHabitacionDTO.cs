namespace LaRicaNoche.Api.DTOs.Request.Update;

public class UpdateTipoHabitacionDto
{
    public string Nombre { get; set; } = string.Empty;
    public int Capacidad { get; set; }
    public string? Descripcion { get; set; }
    public decimal PrecioBase { get; set; }
}