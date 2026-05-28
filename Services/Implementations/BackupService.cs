using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using HotelGenericoApi.Services.Interfaces;

namespace HotelGenericoApi.Services.Implementations;

public class BackupService : IBackupService
{
    private readonly string _connectionString;
    private readonly string _databaseName;

    public BackupService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        // Extraer el nombre de la base de datos de la cadena de conexión
        var builder = new SqlConnectionStringBuilder(_connectionString);
        _databaseName = builder.InitialCatalog;
    }

    public async Task<string> CreateBackupAsync()
    {
        var backupFileName = $"{_databaseName}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.bak";
        var backupPath = Path.Combine(Path.GetTempPath(), backupFileName);

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        // Usar una copia de la cadena de conexión que apunte a master para el comando BACKUP
        var masterConnectionString = new SqlConnectionStringBuilder(_connectionString)
        {
            InitialCatalog = "master"
        }.ConnectionString;

        using var masterConnection = new SqlConnection(masterConnectionString);
        await masterConnection.OpenAsync();

        var commandText = $@"
            BACKUP DATABASE [{_databaseName}]
            TO DISK = @BackupPath
            WITH FORMAT, INIT, NAME = 'HotelGenerico-Full Backup';
        ";

        using var command = new SqlCommand(commandText, masterConnection);
        command.Parameters.AddWithValue("@BackupPath", backupPath);
        await command.ExecuteNonQueryAsync();

        return backupPath;
    }
}
