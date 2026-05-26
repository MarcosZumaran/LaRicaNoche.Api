using HotelGenericoApi.Models;

namespace HotelGenericoApi.Services.Interfaces;

public interface ICategoriaProductoService
{
    Task<IEnumerable<CategoriaProducto>> GetAllAsync();
}
