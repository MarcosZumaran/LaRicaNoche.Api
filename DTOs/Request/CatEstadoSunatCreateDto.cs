namespace HotelGenericoApi.DTOs.Request;

public sealed record CatEstadoSunatCreateDto
{
    public int Codigo { get; init; }
    public string Descripcion { get; init; } = string.Empty;
    public string? DescripcionLarga { get; init; }
}