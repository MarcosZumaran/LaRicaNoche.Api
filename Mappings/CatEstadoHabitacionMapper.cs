using HotelGenericoApi.Models;
using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;
using Riok.Mapperly.Abstractions;

namespace HotelGenericoApi.Mappings;

[Mapper]
public partial class CatEstadoHabitacionMapper
{
    public partial CatEstadoHabitacionResponseDto ToResponse(CatEstadoHabitacion entity);
    public partial CatEstadoHabitacion FromCreate(CatEstadoHabitacionCreateDto dto);
    public partial void UpdateFromDto(CatEstadoHabitacionUpdateDto dto, CatEstadoHabitacion entity);
}