namespace HotelGenericoApi.Models;

public class TransicionEstado
{
    public int IdTransicion { get; set; }
    public int IdEstadoActual { get; set; }
    public int IdEstadoSiguiente { get; set; }

    // Navegación
    public EstadoHabitacion? EstadoActual { get; set; }
    public EstadoHabitacion? EstadoSiguiente { get; set; }
}