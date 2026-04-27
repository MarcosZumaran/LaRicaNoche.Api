using LaRicaNoche.Api.DTOs.Request.Create;
using LaRicaNoche.Api.DTOs.Request.Update;
using LaRicaNoche.Api.DTOs.Response;

namespace LaRicaNoche.Api.Services.Interfaces;

public interface ITipoHabitacionService
{
    Task<List<TipoHabitacionResponseDto>> GetAllAsync();
    Task<TipoHabitacionResponseDto?> GetByIdAsync(int id);
    Task<TipoHabitacionResponseDto> CreateAsync(CreateTipoHabitacionDto dto);
    Task<TipoHabitacionResponseDto?> UpdateAsync(int id, UpdateTipoHabitacionDto dto);
    Task<bool> DeleteAsync(int id);
}