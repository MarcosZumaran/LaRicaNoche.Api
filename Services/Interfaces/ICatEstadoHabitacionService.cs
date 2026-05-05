using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;

namespace HotelGenericoApi.Services.Interfaces;

public interface ICatEstadoHabitacionService
{
    Task<IEnumerable<CatEstadoHabitacionResponseDto>> GetAllAsync();
    Task<CatEstadoHabitacionResponseDto?> GetByIdAsync(int id);
    Task<CatEstadoHabitacionResponseDto> CreateAsync(CatEstadoHabitacionCreateDto dto);
    Task<bool> UpdateAsync(int id, CatEstadoHabitacionUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}