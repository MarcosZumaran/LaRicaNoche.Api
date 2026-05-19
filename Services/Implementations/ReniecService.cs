using Microsoft.Extensions.Logging;
using HotelGenericoApi.Services.Interfaces;
using HotelGenericoApi.Models.Exceptions;

namespace HotelGenericoApi.Services.Implementations;

public class ReniecService : IReniecService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ReniecService> _logger;

    public ReniecService(HttpClient httpClient, ILogger<ReniecService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string?> ConsultarDniAsync(string dni)
    {
        _logger.LogInformation("Consultando RENIEC para DNI {Dni}", dni);
        try
        {
            var response = await _httpClient.GetAsync($"/dni/{dni}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("RENIEC: DNI {Dni} no encontrado", dni);
                return null;
            }
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Consulta RENIEC exitosa para DNI {Dni}", dni);
            return content;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error al consultar RENIEC para DNI {Dni}", dni);
            throw new ExternalServiceException("Error al comunicarse con RENIEC", ex);
        }
    }
}
