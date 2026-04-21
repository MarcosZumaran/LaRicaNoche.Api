using BCrypt.Net;

namespace LaRicaNoche.Api.Security;

public static class PasswordHasher
{
    // Transforma "admin123" en algo como "a3..."
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
    }

    // Compara la contraseña que entra con el hash guardado en la DB
    public static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
}
