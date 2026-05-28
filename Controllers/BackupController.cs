using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelGenericoApi.Services.Interfaces;

namespace HotelGenericoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class BackupController : ControllerBase
{
    private readonly IBackupService _backupService;

    public BackupController(IBackupService backupService) => _backupService = backupService;

    [HttpPost("full")]
    public async Task<IActionResult> CreateFullBackup()
    {
        try
        {
            var filePath = await _backupService.CreateBackupAsync("Full");
            await Task.Delay(500); // Pequeña pausa para que SQL Server libere el archivo
            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var fileName = Path.GetFileName(filePath);
            System.IO.File.Delete(filePath);
            return File(bytes, "application/octet-stream", fileName);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = $"Error al crear el backup: {ex.Message}" });
        }
    }

    [HttpPost("differential")]
    public async Task<IActionResult> CreateDifferentialBackup()
    {
        try
        {
            var filePath = await _backupService.CreateBackupAsync("Differential");
            await Task.Delay(500);
            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var fileName = Path.GetFileName(filePath);
            System.IO.File.Delete(filePath);
            return File(bytes, "application/octet-stream", fileName);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = $"Error al crear el backup diferencial: {ex.Message}" });
        }
    }

    [HttpPost("log")]
    public async Task<IActionResult> CreateLogBackup()
    {
        try
        {
            var filePath = await _backupService.CreateBackupAsync("Log");
            await Task.Delay(500);
            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var fileName = Path.GetFileName(filePath);
            System.IO.File.Delete(filePath);
            return File(bytes, "application/octet-stream", fileName);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = $"Error al crear el backup de log: {ex.Message}" });
        }
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetBackupHistory()
    {
        try
        {
            var history = await _backupService.GetBackupHistoryAsync();
            return Ok(history);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = $"Error al obtener el historial: {ex.Message}" });
        }
    }
}
