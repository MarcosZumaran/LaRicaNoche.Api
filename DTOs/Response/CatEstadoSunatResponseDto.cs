namespace LaRicaNoche.Api.DTOs.Response;

public sealed record CatEstadoSunatResponseDto(
    int Codigo,
    string Descripcion,
    string? DescripcionLarga
);