using LaRicaNoche.Api.Models;
using LaRicaNoche.Api.DTOs.Request;
using LaRicaNoche.Api.DTOs.Response;
using Riok.Mapperly.Abstractions;

namespace LaRicaNoche.Api.Mappings;

[Mapper]
public partial class TiposHabitacionMapper
{
    public partial TiposHabitacionResponseDto ToResponse(TiposHabitacion entity);
    public partial TiposHabitacion FromCreate(TiposHabitacionCreateDto dto);
    public partial void UpdateFromDto(TiposHabitacionUpdateDto dto, TiposHabitacion entity);
}