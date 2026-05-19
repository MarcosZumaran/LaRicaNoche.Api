namespace HotelGenericoApi.Services.Interfaces;

public interface IReniecService
{
    Task<string?> ConsultarDniAsync(string dni);
}
