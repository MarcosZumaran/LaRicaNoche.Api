using HotelGenericoApi.Models;
using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;
using Riok.Mapperly.Abstractions;

namespace HotelGenericoApi.Mappings;

[Mapper]
public partial class CatAfectacionIgvMapper
{
    public partial CatAfectacionIgvResponseDto ToResponse(CatAfectacionIgv entity);
    public partial CatAfectacionIgv FromCreate(CatAfectacionIgvCreateDto dto);
    public partial void UpdateFromDto(CatAfectacionIgvUpdateDto dto, CatAfectacionIgv entity);
}