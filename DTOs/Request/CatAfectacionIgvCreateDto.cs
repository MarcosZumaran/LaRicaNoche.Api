namespace HotelGenericoApi.DTOs.Request;

public sealed record CatAfectacionIgvCreateDto
{
    public string Codigo { get; init; } = string.Empty;
    public string Descripcion { get; init; } = string.Empty;
}