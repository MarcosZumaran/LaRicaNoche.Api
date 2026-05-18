using HotelGenericoApi.Models;

namespace HotelGenericoApi.Services.Interfaces
{
    public interface IHabitacionService
    {
        Task<List<Habitacion>> GetAllAsync();
        Task<Habitacion?> GetByIdAsync(int id);
        Task<Habitacion> CreateAsync(Habitacion habitacion);
        Task<Habitacion?> UpdateAsync(int id, Habitacion habitacionActualizada);
        Task<bool> DeleteAsync(int id);
        Task<bool> CambiarEstadoAsync(int idHabitacion, int idNuevoEstado, int idUsuario, string? observacion = null);
    }
}
