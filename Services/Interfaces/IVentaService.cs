using HotelGenericoApi.Models;

namespace HotelGenericoApi.Services.Interfaces
{
    public interface IVentaService
    {
        Task<List<Venta>> GetAllAsync();
        Task<Venta?> GetByIdAsync(int id);
        Task<Venta> CreateAsync(Venta venta);
        Task<bool> DeleteAsync(int id);
    }
}
