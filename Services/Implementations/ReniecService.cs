using Microsoft.Extensions.Logging;
using HotelGenericoApi.Services.Interfaces;

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

    public async Task<string> ConsultarDniAsync(string dni)
    {
        _logger.LogInformation("Consultando RENIEC para DNI {Dni}", dni);
        var response = await _httpClient.GetAsync($"/dni/{dni}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        _logger.LogInformation("Consulta RENIEC exitosa para DNI {Dni}", dni);
        return content;
    }
}
