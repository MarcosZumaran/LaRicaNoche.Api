namespace HotelGenericoApi.DTOs.Request;

public sealed record CheckInDto
{
    public int IdHabitacion { get; init; }
    public string TipoDocumento { get; init; } = "1";
    public string Documento { get; init; } = string.Empty;
    public string Nombres { get; init; } = string.Empty;
    public string Apellidos { get; init; } = string.Empty;
    public string? Telefono { get; init; }
    public DateTime FechaCheckoutPrevista { get; init; }
    public string MetodoPago { get; init; } = "005"; // Efectivo
    public bool UsarClienteAnonimo { get; init; } = false;
}