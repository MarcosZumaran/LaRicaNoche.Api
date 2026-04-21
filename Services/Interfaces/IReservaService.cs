using LaRicaNoche.Api.DTOs.Base;
using LaRicaNoche.Api.DTOs.Create;

namespace LaRicaNoche.Api.Services.Interfaces;

public interface IReservaService
{
    Task<BaseResponse<List<ReservaResponseDto>>> GetAllAsync();
    Task<BaseResponse<ReservaResponseDto>> GetByIdAsync(int id);
    Task<BaseResponse<ReservaResponseDto>> CreateAsync(CreateReservaDto dto);
    Task<BaseResponse<bool>> FinalizarReservaAsync(int id); // Check-out
}
