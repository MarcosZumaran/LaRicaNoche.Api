using HotelGenericoApi.Models;
using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;
using Riok.Mapperly.Abstractions;

namespace HotelGenericoApi.Mappings;

[Mapper]
public partial class ClienteMapper
{
    public partial ClienteResponseDto ToResponse(Cliente entity);

    // Ignorar propiedades que no se envían al crear
    [MapperIgnoreTarget(nameof(Cliente.IdCliente))]
    [MapperIgnoreTarget(nameof(Cliente.FechaRegistro))]
    [MapperIgnoreTarget(nameof(Cliente.FechaVerificacionReniec))]
    [MapperIgnoreTarget(nameof(Cliente.TipoDocumentoNavigation))]
    [MapperIgnoreTarget(nameof(Cliente.Estancia))]
    [MapperIgnoreTarget(nameof(Cliente.Huespedes))]
    [MapperIgnoreTarget(nameof(Cliente.Reservas))]
    [MapperIgnoreTarget(nameof(Cliente.Venta))]
    public partial Cliente FromCreate(ClienteCreateDto dto);

    // En actualización no se permite modificar TipoDocumento ni Documento
    [MapperIgnoreTarget(nameof(Cliente.IdCliente))]
    [MapperIgnoreTarget(nameof(Cliente.TipoDocumento))]
    [MapperIgnoreTarget(nameof(Cliente.Documento))]
    [MapperIgnoreTarget(nameof(Cliente.FechaRegistro))]
    [MapperIgnoreTarget(nameof(Cliente.FechaVerificacionReniec))]
    [MapperIgnoreTarget(nameof(Cliente.TipoDocumentoNavigation))]
    [MapperIgnoreTarget(nameof(Cliente.Estancia))]
    [MapperIgnoreTarget(nameof(Cliente.Huespedes))]
    [MapperIgnoreTarget(nameof(Cliente.Reservas))]
    [MapperIgnoreTarget(nameof(Cliente.Venta))]
    public partial void UpdateFromDto(ClienteUpdateDto dto, Cliente entity);
}