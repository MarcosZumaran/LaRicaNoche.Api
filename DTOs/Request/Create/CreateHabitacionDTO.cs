namespace LaRicaNoche.Api.DTOs.Request.Create;

public class CreateHabitacionDto
{
    public string NumeroHabitacion {get; set;} = string.Empty;
    public int? Piso {get; set;}
    public string? Descripcion {get; set;}
    public decimal PrecioNoche {get; set;}
    public int IdTipo {get; set;} // Fk con TipoHabitacion
    public int IdEstado {get; set;} = 1; // 1 es igual a Disponible por defecto
}