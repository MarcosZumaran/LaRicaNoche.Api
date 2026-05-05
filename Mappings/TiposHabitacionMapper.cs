using HotelGenericoApi.Models;
using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;
using Riok.Mapperly.Abstractions;

namespace HotelGenericoApi.Mappings;

[Mapper]
public partial class TiposHabitacionMapper
{
    public partial TiposHabitacionResponseDto ToResponse(TiposHabitacion entity);
    public partial TiposHabitacion FromCreate(TiposHabitacionCreateDto dto);
    public partial void UpdateFromDto(TiposHabitacionUpdateDto dto, TiposHabitacion entity);
}