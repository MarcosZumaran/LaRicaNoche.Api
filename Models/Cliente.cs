using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaRicaNoche.Api.Models;

[Table("clientes")]
public class Cliente
{
    [Key]
    [Column("id_cliente")]
    public int IdCliente { get; set; }

    [MaxLength(20)]
    [Column("tipo_documento")]
    public string? TipoDocumento { get; set; }

    [MaxLength(20)]
    [Column("documento")]
    public string? Documento { get; set; }

    [MaxLength(100)]
    [Column("nombres")]
    public string? Nombres { get; set; }

    [MaxLength(100)]
    [Column("apellidos")]
    public string? Apellidos { get; set; }

    [MaxLength(50)]
    [Column("nacionalidad")]
    public string? Nacionalidad { get; set; }

    [Column("fecha_nacimiento")]
    public DateTime? FechaNacimiento { get; set; }

    [MaxLength(15)]
    [Column("telefono")]
    public string? Telefono { get; set; }

    [MaxLength(100)]
    [Column("email")]
    public string? Email { get; set; }

    [MaxLength(200)]
    [Column("direccion")]
    public string? Direccion { get; set; }

    [Column("fecha_registro")]
    public DateTime? FechaRegistro { get; set; }
}