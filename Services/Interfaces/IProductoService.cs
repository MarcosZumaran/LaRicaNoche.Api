using LaRicaNoche.Api.DTOs.Base;
using LaRicaNoche.Api.DTOs.Create;
using LaRicaNoche.Api.DTOs.Update;

namespace LaRicaNoche.Api.Services.Interfaces;

public interface IProductoService
{
    Task<BaseResponse<List<ProductoResponseDto>>> GetAllAsync();
    Task<BaseResponse<ProductoResponseDto>> GetByIdAsync(int id);
    Task<BaseResponse<List<ProductoResponseDto>>> GetByCategoriaAsync(int idCategoria);
    Task<BaseResponse<List<ProductoResponseDto>>> GetConStockAsync();
    Task<BaseResponse<ProductoResponseDto>> CreateAsync(CreateProductoDto dto);
    Task<BaseResponse<bool>> UpdateAsync(int id, UpdateProductoDto dto);
    Task<BaseResponse<bool>> DeleteAsync(int id);
    Task<BaseResponse<bool>> AjustarStockAsync(int id, int cantidad);
}