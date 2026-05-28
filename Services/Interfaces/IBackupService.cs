namespace HotelGenericoApi.Services.Interfaces;

public interface IBackupService
{
    Task<string> CreateBackupAsync();
}
