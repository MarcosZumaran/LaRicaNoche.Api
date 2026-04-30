using LaRicaNoche.Api.Models;
using LaRicaNoche.Api.DTOs.Request;
using LaRicaNoche.Api.DTOs.Response;
using Riok.Mapperly.Abstractions;

namespace LaRicaNoche.Api.Mappings;

[Mapper]
public partial class ProductoMapper
{

    [MapperIgnoreTarget(nameof(Producto.IdProducto))]
    [MapperIgnoreTarget(nameof(Producto.CreatedAt))]
    [MapperIgnoreTarget(nameof(Producto.IdAfectacionIgvNavigation))]
    [MapperIgnoreTarget(nameof(Producto.ItemsVenta))]
    public partial Producto FromCreate(ProductoCreateDto dto);

    [MapperIgnoreTarget(nameof(Producto.IdProducto))]
    [MapperIgnoreTarget(nameof(Producto.CreatedAt))]
    [MapperIgnoreTarget(nameof(Producto.IdAfectacionIgvNavigation))]
    [MapperIgnoreTarget(nameof(Producto.ItemsVenta))]
    public partial void UpdateFromDto(ProductoUpdateDto dto, Producto entity);
}