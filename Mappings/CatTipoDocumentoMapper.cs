using HotelGenericoApi.Models;
using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;
using Riok.Mapperly.Abstractions;

namespace HotelGenericoApi.Mappings;

[Mapper]
public partial class CatTipoDocumentoMapper
{
    public partial CatTipoDocumentoResponseDto ToResponse(CatTipoDocumento entity);
    public partial CatTipoDocumento FromCreate(CatTipoDocumentoCreateDto dto);
    public partial void UpdateFromDto(CatTipoDocumentoUpdateDto dto, CatTipoDocumento entity);
}