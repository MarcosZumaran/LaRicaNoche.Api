namespace HotelGenericoApi.Models;

public class Cliente
{
    public int IdCliente { get; set; }
    public string TipoDocumento { get; set; } = null!; // FK
    public string Documento { get; set; } = null!;
    public string Nombres { get; set; } = null!;
    public string Apellidos { get; set; } = null!;
    public string? Nacionalidad { get; set; }
    public DateOnly? FechaNacimiento { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
    public DateTime? FechaRegistro { get; set; }
    public DateTime? FechaVerificacionReniec { get; set; }

    // Navegación
    public TipoDocumento? TipoDocumentoNavigation { get; set; }
}