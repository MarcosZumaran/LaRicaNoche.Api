namespace HotelGenericoApi.DTOs.Request;

public sealed record ConsumoEstanciaCreateDto
{
    public int IdEstancia { get; init; }
    public int IdProducto { get; init; }
    public int Cantidad { get; init; } = 1;
}