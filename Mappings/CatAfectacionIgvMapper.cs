using LaRicaNoche.Api.Models;
using LaRicaNoche.Api.DTOs.Request;
using LaRicaNoche.Api.DTOs.Response;
using Riok.Mapperly.Abstractions;

namespace LaRicaNoche.Api.Mappings;

[Mapper]
public partial class CatAfectacionIgvMapper
{
    public partial CatAfectacionIgvResponseDto ToResponse(CatAfectacionIgv entity);
    public partial CatAfectacionIgv FromCreate(CatAfectacionIgvCreateDto dto);
    public partial void UpdateFromDto(CatAfectacionIgvUpdateDto dto, CatAfectacionIgv entity);
}