namespace HotelGenericoApi.DTOs.Request;

public sealed record ActualizarConsumoDto
{
    public int Cantidad { get; init; }
}