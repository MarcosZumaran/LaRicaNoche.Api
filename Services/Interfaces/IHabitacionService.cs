using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;

namespace HotelGenericoApi.Services.Interfaces;

public interface IHabitacionService
{
    Task<IEnumerable<HabitacionResponseDto>> GetAllAsync();
    Task<HabitacionResponseDto?> GetByIdAsync(int id);
    Task<HabitacionResponseDto> CreateAsync(HabitacionCreateDto dto, int? idUsuario);
    Task<IEnumerable<HabitacionEstadoActualDto>> GetEstadoActualAsync(string? rolUsuario);
    Task<bool> UpdateAsync(int id, HabitacionUpdateDto dto, int? idUsuario);
    Task<bool> DeleteAsync(int id);
}