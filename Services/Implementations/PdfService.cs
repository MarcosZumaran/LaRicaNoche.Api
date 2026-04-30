using LaRicaNoche.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Playwright;
using LaRicaNoche.Api.Services.Interfaces;

namespace LaRicaNoche.Api.Services.Implementations;

public class PdfService : IPdfService
{
    private readonly LaRicaNocheDbContext _db;

    public PdfService(LaRicaNocheDbContext db)
    {
        _db = db;
    }

    public async Task<byte[]> GenerarPdfComprobanteAsync(int idComprobante)
    {
        var comp = await _db.Comprobantes
            .FirstOrDefaultAsync(c => c.IdComprobante == idComprobante);

        if (comp == null)
            throw new InvalidOperationException("Comprobante no encontrado.");

        // Obtener datos relacionados manualmente
        string? numeroHabitacion = null;
        string? fechasHospedaje = null;

        if (comp.IdEstancia.HasValue)
        {
            var estancia = await _db.Estancias
                .Include(e => e.IdHabitacionNavigation)
                .FirstOrDefaultAsync(e => e.IdEstancia == comp.IdEstancia.Value);

            if (estancia != null)
            {
                numeroHabitacion = estancia.IdHabitacionNavigation?.NumeroHabitacion;
                fechasHospedaje = $"{estancia.FechaCheckin:dd/MM/yyyy} - {estancia.FechaCheckoutPrevista:dd/MM/yyyy}";
            }
        }

        List<Models.ItemsVentum>? itemsVenta = null;
        if (comp.IdVenta.HasValue)
        {
            var venta = await _db.Ventas
                .Include(v => v.ItemsVenta)
                    .ThenInclude(i => i.IdProductoNavigation)
                .FirstOrDefaultAsync(v => v.IdVenta == comp.IdVenta.Value);

            itemsVenta = venta?.ItemsVenta.ToList();
        }

        var html = GenerarHtmlComprobante(comp, numeroHabitacion, fechasHospedaje, itemsVenta);

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions { Headless = true });
        var page = await browser.NewPageAsync();
        await page.SetContentAsync(html);
        var pdfBytes = await page.PdfAsync(new PagePdfOptions
        {
            Format = "A5",
            PrintBackground = true
        });

        return pdfBytes;
    }

    public async Task<byte[]> GenerarPdfVentaAsync(int idVenta)
    {
        var comp = await _db.Comprobantes
            .FirstOrDefaultAsync(c => c.IdVenta == idVenta);

        if (comp == null)
            throw new InvalidOperationException("Comprobante no encontrado para esta venta.");

        return await GenerarPdfComprobanteAsync(comp.IdComprobante);
    }

    public async Task<byte[]> GenerarPdfEstanciaAsync(int idEstancia)
    {
        var comp = await _db.Comprobantes
            .FirstOrDefaultAsync(c => c.IdEstancia == idEstancia);

        if (comp == null)
            throw new InvalidOperationException("Comprobante no encontrado para esta estancia.");

        return await GenerarPdfComprobanteAsync(comp.IdComprobante);
    }

    private string GenerarHtmlComprobante(
        Models.Comprobante comp,
        string? numeroHabitacion,
        string? fechasHospedaje,
        List<Models.ItemsVentum>? itemsVenta)
    {
        string tipo = comp.TipoComprobante == "03" ? "BOLETA DE VENTA" : "FACTURA";
        string cliente = comp.ClienteNombre ?? "CLIENTE ANÓNIMO";
        string doc = comp.ClienteDocumentoNum ?? "—";
        string metodo = comp.MetodoPagoNavigation?.Descripcion ?? comp.MetodoPago ?? "—";

        string itemsHtml = "";

        if (numeroHabitacion != null && fechasHospedaje != null)
        {
            itemsHtml += $@"
            <tr>
                <td>Hospedaje {numeroHabitacion} ({fechasHospedaje})</td>
                <td>1</td>
                <td>{comp.MontoTotal - comp.IgvMonto:F2}</td>
                <td>{comp.MontoTotal:F2}</td>
            </tr>";
        }

        if (itemsVenta != null)
        {
            foreach (var item in itemsVenta)
            {
                itemsHtml += $@"
                <tr>
                    <td>{item.IdProductoNavigation?.Nombre}</td>
                    <td>{item.Cantidad}</td>
                    <td>{item.PrecioUnitario:F2}</td>
                    <td>{item.Subtotal:F2}</td>
                </tr>";
            }
        }

        return $@"
            <!DOCTYPE html>
            <html><head><meta charset='utf-8'><style>
            body {{ font-family: Arial, sans-serif; margin: 20px; font-size: 12px; }}
            h2 {{ text-align: center; }}
            table {{ width: 100%; border-collapse: collapse; }}
            th, td {{ border: 1px solid #ccc; padding: 4px; text-align: left; }}
            th {{ background-color: #f2f2f2; }}
            .right {{ text-align: right; }}
            .total {{ font-weight: bold; }}
            </style></head><body>
            <h2>{tipo}</h2>
            <p><strong>Serie/Número:</strong> {comp.Serie}-{comp.Correlativo}</p>
            <p><strong>Fecha:</strong> {comp.FechaEmision:dd/MM/yyyy}</p>
            <p><strong>Cliente:</strong> {cliente} | <strong>Doc:</strong> {doc}</p>
            <p><strong>Método de pago:</strong> {metodo}</p>
            <table>
                <thead><tr><th>Descripción</th><th>Cant.</th><th>P.Unit.</th><th>Subtotal</th></tr></thead>
                <tbody>{itemsHtml}</tbody>
            </table>
            <p class='right'>Subtotal: S/ {comp.MontoTotal - comp.IgvMonto:F2}</p>
            <p class='right'>IGV (10.5%): S/ {comp.IgvMonto:F2}</p>
            <h3 class='right'>TOTAL: S/ {comp.MontoTotal:F2}</h3>
            </body></html>";
    }

}