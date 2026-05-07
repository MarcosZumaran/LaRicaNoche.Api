namespace HotelGenericoApi.Services.Interfaces;

public interface IValidadorEstadoService
{
    Task<bool> EsTransicionValidaAsync(int idEstadoActual, int idEstadoSiguiente);
}