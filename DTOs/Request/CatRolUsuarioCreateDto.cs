namespace LaRicaNoche.Api.DTOs.Request;

public sealed record CatRolUsuarioCreateDto
{
    public string Nombre { get; init; } = string.Empty;
}