using LaRicaNoche.Api.DTOs.Base;
using LaRicaNoche.Api.DTOs.Create;

namespace LaRicaNoche.Api.Services.Interfaces;

public interface IComprobanteService
{
    Task<BaseResponse<List<ComprobanteResponseDto>>> GetAllAsync();
    Task<BaseResponse<List<ComprobanteResponseDto>>> GetByFechaAsync(DateTime fecha);
    Task<BaseResponse<ComprobanteResponseDto>> GetByIdAsync(int id);
    Task<BaseResponse<ComprobanteResponseDto>> GetByReferenciaAsync(int idReferencia, string tipo);
    Task<BaseResponse<ComprobanteResponseDto>> CreateAsync(CreateComprobanteDto dto);
    Task<BaseResponse<ComprobanteResponseDto>> AlternarEstadoAsync(int id);
}