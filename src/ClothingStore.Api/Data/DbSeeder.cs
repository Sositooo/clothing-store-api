using ClothingStore.Api.Models;
using ClothingStore.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Api.Data;

/// <summary>
/// Создаёт таблицы (применяет миграции) и добавляет начальные данные:
/// менеджера (через регистрацию можно стать только обычным пользователем) и несколько футболок.
/// </summary>
public static class DbSeeder
{
    public static async Task MigrateAndSeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<PasswordHasher>();

        await db.Database.MigrateAsync();

        if (!await db.Users.AnyAsync(u => u.Role == UserRole.Manager))
        {
            db.Users.Add(new User
            {
                Login = "manager",
                PasswordHash = hasher.Hash("Manager123!"),
                Phone = "+70000000000",
                DeliveryAddress = "Магазин",
                Role = UserRole.Manager
            });
        }

        if (!await db.Clothes.AnyAsync())
        {
            db.Clothes.AddRange(
                Make("Футболка белая", 990, "Хлопок 100%", ClothingType.TShirt),
                Make("Футболка чёрная", 990, "Хлопок 100%", ClothingType.TShirt),
                Make("Футболка с котиком", 1490, "Свободный крой, принт", ClothingType.TShirt),
                Make("Худи серое", 3490, "Футер с начёсом", ClothingType.Hoodie));
        }

        await db.SaveChangesAsync();
    }

    private static Clothing Make(string name, decimal price, string description, ClothingType type) => new()
    {
        Name = name,
        Price = price,
        Description = description,
        Type = type,
        Sizes =
        [
            new() { Letter = "S", Number = 44, Quantity = 10 },
            new() { Letter = "M", Number = 46, Quantity = 10 },
            new() { Letter = "L", Number = 48, Quantity = 10 },
            new() { Letter = "XL", Number = 50, Quantity = 10 }
        ]
    };
}
