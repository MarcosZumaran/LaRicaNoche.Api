namespace LaRicaNoche.Api.DTOs.Base;

public class HabitacionResponseDto
{
    public int IdHabitacion { get; set; }
    public string NumeroHabitacion { get; set; } = string.Empty;
    public int Piso { get; set; }
    public decimal PrecioNoche { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime? FechaUltimoCheckout { get; set; }
}