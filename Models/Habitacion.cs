using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaRicaNoche.Api.Models;

[Table("habitaciones")]
public class Habitacion
{
    [Key]
    [Column("id_habitacion")]
    public int IdHabitacion { get; set; }

    [Required, MaxLength(10)]
    [Column("numero_habitacion")]
    public string NumeroHabitacion { get; set; } = string.Empty;

    [Column("piso")]
    public int Piso { get; set; } = 1;

    [Column("precio_noche")]
    public decimal PrecioNoche { get; set; } = 50.00m;

    [MaxLength(20)]
    [Column("estado")]
    public string? Estado { get; set; }

    [Column("fecha_ultimo_checkout")]
    public DateTime? FechaUltimoCheckout { get; set; }
}