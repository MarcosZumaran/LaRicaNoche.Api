using LaRicaNoche.Api.DTOs.Base;
using LaRicaNoche.Api.DTOs.Create;

namespace LaRicaNoche.Api.Services.Interfaces;

public interface ICategoriaService
{
    Task<BaseResponse<List<CategoriaResponseDto>>> GetAllAsync();
    Task<BaseResponse<CategoriaResponseDto>> GetByIdAsync(int id);
    Task<BaseResponse<CategoriaResponseDto>> CreateAsync(CreateCategoriaDto dto);
    Task<BaseResponse<bool>> UpdateAsync(int id, CreateCategoriaDto dto);
    Task<BaseResponse<bool>> DeleteAsync(int id);
}