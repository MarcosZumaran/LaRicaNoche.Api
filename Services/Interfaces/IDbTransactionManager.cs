namespace HotelGenericoApi.Services.Interfaces;

public interface IDbTransactionManager
{
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    Task DisposeAsync();
}