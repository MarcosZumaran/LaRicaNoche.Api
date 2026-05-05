namespace HotelGenericoApi.DTOs.Request;

public sealed record CatTipoComprobanteCreateDto
{
    public string Codigo { get; init; } = string.Empty;
    public string Descripcion { get; init; } = string.Empty;
}