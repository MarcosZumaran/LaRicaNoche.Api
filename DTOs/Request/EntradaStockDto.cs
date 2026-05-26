using System.ComponentModel.DataAnnotations;

namespace HotelGenericoApi.DTOs.Request;

public sealed record EntradaStockDto
{
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
    public int Cantidad { get; init; }
}
