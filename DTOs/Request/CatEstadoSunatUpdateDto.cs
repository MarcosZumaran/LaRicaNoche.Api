namespace LaRicaNoche.Api.DTOs.Request;

public sealed record CatEstadoSunatUpdateDto
{
    public string? Descripcion { get; init; }
    public string? DescripcionLarga { get; init; }
}