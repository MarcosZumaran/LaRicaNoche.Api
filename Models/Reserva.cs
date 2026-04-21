using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaRicaNoche.Api.Models;

[Table("reservas")]
public class Reserva
{
    [Key]
    [Column("id_reserva")]
    public int IdReserva { get; set; }

    [Column("id_cliente")]
    public int IdCliente { get; set; }

    [Column("id_habitacion")]
    public int IdHabitacion { get; set; }

    [Column("id_usuario_recepcion")]
    public int IdUsuarioRecepcion { get; set; }

    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    [Column("fecha_entrada")]
    public DateTime FechaEntrada { get; set; }

    [Column("fecha_salida")]
    public DateTime FechaSalida { get; set; }

    [Column("monto_total")]
    public decimal MontoTotal { get; set; }

    [MaxLength(20)]
    [Column("metodo_pago")]
    public string? MetodoPago { get; set; }

    [MaxLength(20)]
    [Column("estado_reserva")]
    public string? EstadoReserva { get; set; }

    [MaxLength(20)]
    [Column("num_boleta")]
    public string? NumBoleta { get; set; }

    [ForeignKey("IdCliente")]
    public Cliente? Cliente { get; set; }

    [ForeignKey("IdHabitacion")]
    public Habitacion? Habitacion { get; set; }
}