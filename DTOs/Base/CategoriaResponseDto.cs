namespace LaRicaNoche.Api.DTOs.Base;

public class CategoriaResponseDto
{
    public int IdCategoria { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}