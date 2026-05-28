namespace HotelGenericoApi.DTOs.Response;

public class BackupHistoryDto
{
    public string DatabaseName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Full, Differential, Log
    public DateTime BackupStartDate { get; set; }
    public DateTime BackupFinishDate { get; set; }
    public long BackupSize { get; set; } // bytes
    public string PhysicalDeviceName { get; set; } = string.Empty;
    public int BackupSetId { get; set; }
}
