using LaRicaNoche.Api.DTOs.Base;
using LaRicaNoche.Api.DTOs.Create;
using LaRicaNoche.Api.DTOs.Update;

namespace LaRicaNoche.Api.Services.Interfaces;

public interface IHabitacionService
{
    Task<BaseResponse<List<HabitacionResponseDto>>> GetAllAsync();
    Task<BaseResponse<HabitacionResponseDto>> GetByIdAsync(int id);
    Task<BaseResponse<HabitacionResponseDto>> CreateAsync(CreateHabitacionDto dto);
    Task<BaseResponse<bool>> UpdateAsync(int id, UpdateHabitacionDto dto);
    Task<BaseResponse<bool>> MarcarLimpiezaCompletadaAsync(int id);
}
