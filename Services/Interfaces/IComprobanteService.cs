using LaRicaNoche.Api.DTOs.Response;

namespace LaRicaNoche.Api.Services.Interfaces;

public interface IComprobanteService
{
    Task<IEnumerable<ComprobanteResponseDto>> GetAllAsync();
    Task<ComprobanteResponseDto?> GetByIdAsync(int id);
    Task<bool> MarcarComoEnviadoAsync(int id, string hashXml);
}