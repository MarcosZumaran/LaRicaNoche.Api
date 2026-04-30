namespace LaRicaNoche.Api.Services.Interfaces;
public interface IPdfService
{
    Task<byte[]> GenerarPdfComprobanteAsync(int idComprobante);
}