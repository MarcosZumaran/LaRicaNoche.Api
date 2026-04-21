using LaRicaNoche.Api.DTOs.Base;
using LaRicaNoche.Api.DTOs.Create;
using LaRicaNoche.Api.DTOs.Update;

namespace LaRicaNoche.Api.Services.Interfaces;

public interface IClienteService
{
    Task<BaseResponse<List<ClienteResponseDto>>> GetAllAsync();
    Task<BaseResponse<ClienteResponseDto>> GetByIdAsync(int id);
    Task<BaseResponse<ClienteResponseDto>> GetByDocumentoAsync(string documento);
    Task<BaseResponse<ClienteResponseDto>> CreateAsync(CreateClienteDto dto);
    Task<BaseResponse<bool>> UpdateAsync(int id, UpdateClienteDto dto);
    Task<BaseResponse<bool>> DeleteAsync(int id);
}
