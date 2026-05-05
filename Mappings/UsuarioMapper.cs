using HotelGenericoApi.Models;
using HotelGenericoApi.DTOs.Request;
using Riok.Mapperly.Abstractions;

namespace HotelGenericoApi.Mappings;

[Mapper]
public partial class UsuarioMapper
{
    // Ignorar password_hash porque se maneja con bcrypt en el servicio
    [MapperIgnoreTarget(nameof(Usuario.PasswordHash))]
    public partial Usuario FromCreate(UsuarioCreateDto dto);

    // Ignorar propiedades que no queremos mapear desde el DTO de actualización
    [MapperIgnoreTarget(nameof(Usuario.PasswordHash))]
    [MapperIgnoreTarget(nameof(Usuario.FechaCreacion))]
    public partial void UpdateFromDto(UsuarioUpdateDto dto, Usuario entity);
}