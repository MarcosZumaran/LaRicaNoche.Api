using System.ComponentModel.DataAnnotations;

namespace HotelGenericoApi.DTOs.Request;

public sealed record LoginDto
{
    [Required]
    public string Username { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;
}