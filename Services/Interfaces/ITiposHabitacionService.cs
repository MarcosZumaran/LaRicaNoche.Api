using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;

namespace HotelGenericoApi.Services.Interfaces;

public interface ITiposHabitacionService
{
    Task<IEnumerable<TiposHabitacionResponseDto>> GetAllAsync();
    Task<TiposHabitacionResponseDto?> GetByIdAsync(int id);
    Task<TiposHabitacionResponseDto> CreateAsync(TiposHabitacionCreateDto dto);
    Task<bool> UpdateAsync(int id, TiposHabitacionUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}