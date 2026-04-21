namespace LaRicaNoche.Api.DTOs.Create;

public class CreateVentaDto
{
    public int? IdCliente { get; set; }
    public int IdUsuario { get; set; }
    public string MetodoPago { get; set; } = "Efectivo";
    public List<CreateItemVentaDto> Items { get; set; } = new();
}

public class CreateItemVentaDto
{
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
}