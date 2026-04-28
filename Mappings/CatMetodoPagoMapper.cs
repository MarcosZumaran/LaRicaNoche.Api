using LaRicaNoche.Api.Models;
using LaRicaNoche.Api.DTOs.Request;
using LaRicaNoche.Api.DTOs.Response;
using Riok.Mapperly.Abstractions;

namespace LaRicaNoche.Api.Mappings;

[Mapper]
public partial class CatMetodoPagoMapper
{
    public partial CatMetodoPagoResponseDto ToResponse(CatMetodoPago entity);
    public partial CatMetodoPago FromCreate(CatMetodoPagoCreateDto dto);
    public partial void UpdateFromDto(CatMetodoPagoUpdateDto dto, CatMetodoPago entity);
}