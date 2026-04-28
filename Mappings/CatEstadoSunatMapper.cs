using LaRicaNoche.Api.Models;
using LaRicaNoche.Api.DTOs.Request;
using LaRicaNoche.Api.DTOs.Response;
using Riok.Mapperly.Abstractions;

namespace LaRicaNoche.Api.Mappings;

[Mapper]
public partial class CatEstadoSunatMapper
{
    public partial CatEstadoSunatResponseDto ToResponse(CatEstadoSunat entity);
    public partial CatEstadoSunat FromCreate(CatEstadoSunatCreateDto dto);
    public partial void UpdateFromDto(CatEstadoSunatUpdateDto dto, CatEstadoSunat entity);
}