namespace HotelGenericoApi.DTOs.Response;

public sealed record ComprobanteResponseDto(
    int IdComprobante,
    int? IdEstancia,
    int? IdVenta,
    string? TipoComprobante,
    string Serie,
    int Correlativo,
    DateTime? FechaEmision,
    decimal MontoTotal,
    decimal IgvMonto,
    string? ClienteDocumentoTipo,
    string? ClienteDocumentoNum,
    string? ClienteNombre,
    string? MetodoPago,
    int? IdEstadoSunat,
    string? NombreEstadoSunat,
    DateTime? FechaEnvio,
    int? IntentosEnvio
);