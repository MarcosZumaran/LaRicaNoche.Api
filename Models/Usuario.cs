namespace HotelGenericoApi.Models;

public class Usuario
{
    public int IdUsuario { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public int IdRol { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public bool? EstaActivo { get; set; }
    public bool DebeCambiarPassword { get; set; } = true;

    // Navegación
    public RolUsuario? Rol { get; set; }
}