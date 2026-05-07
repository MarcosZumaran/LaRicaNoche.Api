namespace HotelGenericoApi.Models;

public partial class CatTransicionEstado
{
    public int IdTransicion { get; set; }
    public int IdEstadoActual { get; set; }
    public int IdEstadoSiguiente { get; set; }

    public virtual CatEstadoHabitacion IdEstadoActualNavigation { get; set; } = null!;
    public virtual CatEstadoHabitacion IdEstadoSiguienteNavigation { get; set; } = null!;
}