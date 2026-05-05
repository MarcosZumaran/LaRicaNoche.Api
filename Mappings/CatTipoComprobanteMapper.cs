using HotelGenericoApi.Models;
using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;
using Riok.Mapperly.Abstractions;

namespace HotelGenericoApi.Mappings;

[Mapper]
public partial class CatTipoComprobanteMapper
{
    public partial CatTipoComprobanteResponseDto ToResponse(CatTipoComprobante entity);
    public partial CatTipoComprobante FromCreate(CatTipoComprobanteCreateDto dto);
    public partial void UpdateFromDto(CatTipoComprobanteUpdateDto dto, CatTipoComprobante entity);
}