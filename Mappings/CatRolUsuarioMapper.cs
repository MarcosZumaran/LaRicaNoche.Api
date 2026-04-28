using LaRicaNoche.Api.Models;
using LaRicaNoche.Api.DTOs.Request;
using LaRicaNoche.Api.DTOs.Response;
using Riok.Mapperly.Abstractions;

namespace LaRicaNoche.Api.Mappings;

[Mapper]
public partial class CatRolUsuarioMapper
{
    public partial CatRolUsuarioResponseDto ToResponse(CatRolUsuario entity);
    public partial CatRolUsuario FromCreate(CatRolUsuarioCreateDto dto);
    public partial void UpdateFromDto(CatRolUsuarioUpdateDto dto, CatRolUsuario entity);
}