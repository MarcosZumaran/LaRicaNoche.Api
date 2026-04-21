using LaRicaNoche.Api.DTOs.Base;
using LaRicaNoche.Api.DTOs.Create;

namespace LaRicaNoche.Api.Services.Interfaces;

public interface IVentaService
{
    Task<BaseResponse<List<VentaResponseDto>>> GetAllAsync();
    Task<BaseResponse<List<VentaResponseDto>>> GetByFechaAsync(DateTime fecha);
    Task<BaseResponse<VentaResponseDto>> GetByIdAsync(int id);
    Task<BaseResponse<VentaResponseDto>> CreateAsync(CreateVentaDto dto);
}