using Microsoft.EntityFrameworkCore;
using HotelGenericoApi.Data;
using HotelGenericoApi.Models;
using HotelGenericoApi.Services.Interfaces;

namespace HotelGenericoApi.Services.Implementations;

public class CategoriaProductoService : ICategoriaProductoService
{
    private readonly HotelDbContext _db;
    public CategoriaProductoService(HotelDbContext db) => _db = db;

    public async Task<IEnumerable<CategoriaProducto>> GetAllAsync()
    {
        return await _db.CategoriasProducto
            .AsNoTracking()
            .ToListAsync();
    }
}
