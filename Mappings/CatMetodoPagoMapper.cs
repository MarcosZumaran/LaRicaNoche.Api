using HotelGenericoApi.Models;
using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;
using Riok.Mapperly.Abstractions;

namespace HotelGenericoApi.Mappings;

[Mapper]
public partial class CatMetodoPagoMapper
{
    public partial CatMetodoPagoResponseDto ToResponse(CatMetodoPago entity);
    public partial CatMetodoPago FromCreate(CatMetodoPagoCreateDto dto);
    public partial void UpdateFromDto(CatMetodoPagoUpdateDto dto, CatMetodoPago entity);
}