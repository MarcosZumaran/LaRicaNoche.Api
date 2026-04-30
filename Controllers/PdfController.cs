using Microsoft.AspNetCore.Mvc;
using LaRicaNoche.Api.Services.Interfaces;

namespace LaRicaNoche.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PdfController : ControllerBase
{
    private readonly IPdfService _pdfService;
    public PdfController(IPdfService pdfService) => _pdfService = pdfService;

    [HttpGet("Comprobante/{id}")]
    public async Task<IActionResult> GetComprobantePdf(int id)
    {
        try
        {
            var bytes = await _pdfService.GenerarPdfComprobanteAsync(id);
            return File(bytes, "application/pdf", $"comprobante_{id}.pdf");
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }
}