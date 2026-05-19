using Microsoft.Extensions.Logging;
using HotelGenericoApi.Services.Interfaces;
using HotelGenericoApi.Models.Exceptions;

namespace HotelGenericoApi.Services.Implementations;

public class ReniecService : IReniecService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ReniecService> _logger;
    private readonly string _apiKey;

    public ReniecService(HttpClient httpClient, ILogger<ReniecService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiKey = configuration["VerificaPE:ApiKey"]!;

        // La base URL la dejamos aquí para documentar, pero no la usaremos como BaseAddress
    }

    public async Task<string?> ConsultarDniAsync(string dni)
    {
        _logger.LogInformation("Consultando RENIEC para DNI {Dni}", dni);

        // Construimos la URL
        var url = $"https://api.verificape.com/v2/dni/{dni}";
        _logger.LogDebug("URL RENIEC: {Url}", url);

        // Creamos una petición independiente para añadir el header de autorización
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

        try
        {
            var response = await _httpClient.SendAsync(request);

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
