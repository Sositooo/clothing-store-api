using System.Text.Json.Serialization;

namespace ClothingStore.Api.Models;

/// <summary>Пользователь (покупатель или менеджер).</summary>
public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;

    /// <summary>Хеш пароля (BCrypt). Пароль в открытом виде не хранится.</summary>
    [JsonIgnore] // хеш пароля никогда не отдаём в ответах API
    public string PasswordHash { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;
    public string DeliveryAddress { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;

    [JsonIgnore]
    public List<Order> Orders { get; set; } = [];
}
