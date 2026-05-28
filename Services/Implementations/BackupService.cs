using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using HotelGenericoApi.DTOs.Response;
using HotelGenericoApi.Services.Interfaces;

namespace HotelGenericoApi.Services.Implementations;

public class BackupService : IBackupService
{
    private readonly string _connectionString;
    private readonly string _databaseName;

    public BackupService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        var builder = new SqlConnectionStringBuilder(_connectionString);
        _databaseName = builder.InitialCatalog;
    }

    public async Task<string> CreateBackupAsync(string type = "Full")
    {
        var backupFileName = $"{_databaseName}_{type}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.bak";
        var backupPath = Path.Combine(Path.GetTempPath(), backupFileName);

        var masterConnectionString = new SqlConnectionStringBuilder(_connectionString)
        {
            InitialCatalog = "master"
        }.ConnectionString;

        using var masterConnection = new SqlConnection(masterConnectionString);
        await masterConnection.OpenAsync();

        var commandText = type switch
        {
            "Full" => $@"BACKUP DATABASE [{_databaseName}] TO DISK = @BackupPath WITH FORMAT, INIT, NAME = 'HotelGenerico-Full Backup', STATS = 10;",
            "Differential" => $@"BACKUP DATABASE [{_databaseName}] TO DISK = @BackupPath WITH DIFFERENTIAL, INIT, NAME = 'HotelGenerico-Diff Backup', STATS = 10;",
            "Log" => $@"BACKUP LOG [{_databaseName}] TO DISK = @BackupPath WITH INIT, NAME = 'HotelGenerico-Log Backup', STATS = 10;",
            _ => throw new ArgumentException("Tipo de backup no soportado.")
        };

        using var command = new SqlCommand(commandText, masterConnection);
        command.Parameters.AddWithValue("@BackupPath", backupPath);
        await command.ExecuteNonQueryAsync();

        return backupPath;
    }

    public async Task<List<BackupHistoryDto>> GetBackupHistoryAsync()
    {
        var history = new List<BackupHistoryDto>();

        var query = @"
            SELECT TOP 20
                bs.database_name AS DatabaseName,
                CASE bs.type
                    WHEN 'D' THEN 'Full'
                    WHEN 'I' THEN 'Differential'
                    WHEN 'L' THEN 'Log'
                END AS Type,
                bs.backup_start_date AS BackupStartDate,
                bs.backup_finish_date AS BackupFinishDate,
                bs.backup_size AS BackupSize,
                bmf.physical_device_name AS PhysicalDeviceName,
                bs.backup_set_id AS BackupSetId
            FROM msdb.dbo.backupset bs
            INNER JOIN msdb.dbo.backupmediafamily bmf ON bs.media_set_id = bmf.media_set_id
            WHERE bs.database_name = @DatabaseName
            ORDER BY bs.backup_start_date DESC;
        ";

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@DatabaseName", _databaseName);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            history.Add(new BackupHistoryDto
            {
                DatabaseName = reader["DatabaseName"].ToString()!,
                Type = reader["Type"].ToString()!,
                BackupStartDate = Convert.ToDateTime(reader["BackupStartDate"]),
                BackupFinishDate = Convert.ToDateTime(reader["BackupFinishDate"]),
                BackupSize = Convert.ToInt64(reader["BackupSize"]),
                PhysicalDeviceName = reader["PhysicalDeviceName"].ToString()!,
                BackupSetId = Convert.ToInt32(reader["BackupSetId"])
            });
        }
        return history;
    }
}
