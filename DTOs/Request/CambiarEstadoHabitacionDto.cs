namespace LaRicaNoche.Api.DTOs.Request;

public class CambiarEstadoHabitacionDto
{
    public int IdNuevoEstado { get; set; }
    public string? Observacion { get; set; }
    public int IdUsuario { get; set; }
}