namespace ClothingStore.Api.Services;

/// <summary>Хеширование паролей через BCrypt — в БД хранится только хеш.</summary>
public class PasswordHasher
{
    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
}
