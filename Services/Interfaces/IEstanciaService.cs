using HotelGenericoApi.Models;

namespace HotelGenericoApi.Services.Interfaces
{
    public interface IEstanciaService
    {
        Task<List<Estancia>> GetAllAsync();
        Task<Estancia?> GetByIdAsync(int id);
        Task<Estancia> CreateAsync(Estancia estancia);
        Task<Estancia?> CheckoutAsync(int idEstancia, int idUsuario);
        Task<bool> AddHuespedAsync(int idEstancia, Huesped huesped);
        Task<bool> AddConsumoAsync(int idEstancia, ItemEstancia item);
    }
}
