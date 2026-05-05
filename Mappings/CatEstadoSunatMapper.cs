using HotelGenericoApi.Models;
using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;
using Riok.Mapperly.Abstractions;

namespace HotelGenericoApi.Mappings;

[Mapper]
public partial class CatEstadoSunatMapper
{
    public partial CatEstadoSunatResponseDto ToResponse(CatEstadoSunat entity);
    public partial CatEstadoSunat FromCreate(CatEstadoSunatCreateDto dto);
    public partial void UpdateFromDto(CatEstadoSunatUpdateDto dto, CatEstadoSunat entity);
}