using HotelGenericoApi.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HotelGenericoApi.Services.Implementations;

public class BackupCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BackupCleanupService> _logger;
    private readonly TimeSpan _intervalo = TimeSpan.FromHours(24);

    public BackupCleanupService(IServiceScopeFactory scopeFactory, ILogger<BackupCleanupService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Servicio de limpieza de backups iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var backupService = scope.ServiceProvider.GetRequiredService<IBackupService>();

                var eliminados = await backupService.LimpiarBackupsAntiguosAsync(30);
                if (eliminados > 0)
                    _logger.LogInformation("Limpieza de backups: {Count} archivos eliminados.", eliminados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en limpieza automática de backups.");
            }

            await Task.Delay(_intervalo, stoppingToken);
        }
    }
}
