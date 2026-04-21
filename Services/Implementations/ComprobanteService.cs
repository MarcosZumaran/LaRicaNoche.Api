using Microsoft.EntityFrameworkCore;
using LaRicaNoche.Api.Data;
using LaRicaNoche.Api.Models;
using LaRicaNoche.Api.DTOs.Base;
using LaRicaNoche.Api.DTOs.Create;
using LaRicaNoche.Api.Services.Interfaces;

namespace LaRicaNoche.Api.Services.Implementations;

public class ComprobanteService : IComprobanteService
{
    private readonly LaRicaNocheDbContext _context;
    private const decimal TASA_IGV = 0.18m;

    public ComprobanteService(LaRicaNocheDbContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse<List<ComprobanteResponseDto>>> GetAllAsync()
    {
        var entities = await _context.Comprobantes
            .OrderByDescending(c => c.FechaEmision)
            .Take(50)
            .ToListAsync();

        var response = entities.Select(c => MapToDto(c)).ToList();
        return new BaseResponse<List<ComprobanteResponseDto>> { Data = response };
    }

    public async Task<BaseResponse<List<ComprobanteResponseDto>>> GetByFechaAsync(DateTime fecha)
    {
        var entities = await _context.Comprobantes
            .Where(c => c.FechaEmision != null && c.FechaEmision.Value.Date == fecha.Date)
            .OrderByDescending(c => c.FechaEmision)
            .ToListAsync();

        var response = entities.Select(c => MapToDto(c)).ToList();
        return new BaseResponse<List<ComprobanteResponseDto>> { Data = response };
    }

    public async Task<BaseResponse<ComprobanteResponseDto>> GetByIdAsync(int id)
    {
        var entity = await _context.Comprobantes.FindAsync(id);
        if (entity == null) return new BaseResponse<ComprobanteResponseDto> { IsSuccess = false, Message = "Comprobante no encontrado" };

        return new BaseResponse<ComprobanteResponseDto> { Data = MapToDto(entity) };
    }

    public async Task<BaseResponse<ComprobanteResponseDto>> GetByReferenciaAsync(int idReferencia, string tipo)
    {
        var entity = await _context.Comprobantes
            .FirstOrDefaultAsync(c => c.IdReferencia == idReferencia && c.TipoReferencia == tipo);

        if (entity == null) return new BaseResponse<ComprobanteResponseDto> { IsSuccess = false, Message = "Comprobante no encontrado" };

        return new BaseResponse<ComprobanteResponseDto> { Data = MapToDto(entity) };
    }

    public async Task<BaseResponse<ComprobanteResponseDto>> CreateAsync(CreateComprobanteDto dto)
    {
        decimal montoTotal = 0;

        if (dto.TipoReferencia == "Reserva")
        {
            var reserva = await _context.Reservas.FindAsync(dto.IdReferencia);
            if (reserva == null) return new BaseResponse<ComprobanteResponseDto> { IsSuccess = false, Message = "Reserva no encontrada" };
            montoTotal = reserva.MontoTotal;
        }
        else if (dto.TipoReferencia == "Venta")
        {
            var venta = await _context.Ventas.FindAsync(dto.IdReferencia);
            if (venta == null) return new BaseResponse<ComprobanteResponseDto> { IsSuccess = false, Message = "Venta no encontrada" };
            montoTotal = venta.TotalVenta;
        }
        else
        {
            return new BaseResponse<ComprobanteResponseDto> { IsSuccess = false, Message = "Tipo de referencia inválido" };
        }

        var ultimo = await _context.Comprobantes
            .Where(c => c.Serie == "B001")
            .OrderByDescending(c => c.Correlativo)
            .FirstOrDefaultAsync();

        var siguienteCorrelativo = ultimo == null ? 1 : ultimo.Correlativo + 1;

        var entity = new Comprobante
        {
            IdReferencia = dto.IdReferencia,
            TipoReferencia = dto.TipoReferencia,
            TipoComprobante = dto.TipoComprobante,
            Serie = "B001",
            Correlativo = siguienteCorrelativo,
            MontoTotal = montoTotal,
            ClienteDocumento = dto.ClienteDocumento,
            ClienteNombres = dto.ClienteNombres,
            EstadoSunat = "Emitido"
        };

        _context.Comprobantes.Add(entity);
        await _context.SaveChangesAsync();

        return new BaseResponse<ComprobanteResponseDto> { Data = MapToDto(entity, dto.ClienteDocumento, dto.ClienteNombres) };
    }

    public async Task<BaseResponse<ComprobanteResponseDto>> AlternarEstadoAsync(int id)
    {
        var entity = await _context.Comprobantes.FindAsync(id);
        if (entity == null) return new BaseResponse<ComprobanteResponseDto> { IsSuccess = false, Message = "Comprobante no encontrado" };

        entity.EstadoSunat = entity.EstadoSunat == "Emitido" ? "Anulado" : "Emitido";
        await _context.SaveChangesAsync();

        return new BaseResponse<ComprobanteResponseDto> { Data = MapToDto(entity) };
    }

    private ComprobanteResponseDto MapToDto(Comprobante c, string? clienteDoc = null, string? clienteNom = null)
    {
        decimal baseImponible = c.MontoTotal / (1 + TASA_IGV);
        decimal igv = c.MontoTotal - baseImponible;

        return new ComprobanteResponseDto
        {
            IdComprobante = c.IdComprobante,
            IdReferencia = c.IdReferencia,
            TipoReferencia = c.TipoReferencia,
            TipoComprobante = c.TipoComprobante,
            Serie = c.Serie,
            Correlativo = c.Correlativo.ToString("D8"),
            NumeroCompleto = $"{c.Serie}-{c.Correlativo:D8}",
            FechaEmision = c.FechaEmision ?? DateTime.Now,
            ClienteDocumento = clienteDoc ?? c.ClienteDocumento ?? "",
            ClienteNombres = clienteNom ?? c.ClienteNombres ?? "",
            Subtotal = Math.Round(baseImponible, 2),
            Igv = Math.Round(igv, 2),
            MontoTotal = c.MontoTotal,
            EstadoSunat = c.EstadoSunat
        };
    }
}