using HotelGenericoApi.DTOs.Response;

namespace HotelGenericoApi.Services.Interfaces;

public interface IBackupService
{
    Task<string> CreateBackupAsync(string type = "Full");
    Task<List<BackupHistoryDto>> GetBackupHistoryAsync();
}
