using HotelGenericoApi.Models;
using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;
using Riok.Mapperly.Abstractions;

namespace HotelGenericoApi.Mappings;

[Mapper]
public partial class CatRolUsuarioMapper
{
    public partial CatRolUsuarioResponseDto ToResponse(CatRolUsuario entity);
    public partial CatRolUsuario FromCreate(CatRolUsuarioCreateDto dto);
    public partial void UpdateFromDto(CatRolUsuarioUpdateDto dto, CatRolUsuario entity);
}