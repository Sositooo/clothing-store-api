using ClothingStore.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Api.Endpoints;

public record UpdateQuantityRequest(int Quantity);

public static class ClothesEndpoints
{
    public static void MapClothesEndpoints(this WebApplication app)
    {
        // Список всей одежды (вместе с размерами)
        app.MapGet("/api/clothes", async (AppDbContext db) =>
            await db.Clothes.Include(c => c.Sizes).OrderBy(c => c.Id).ToListAsync());

        // Поиск по части названия (без учёта регистра)
        app.MapGet("/api/clothes/search", async (string name, AppDbContext db) =>
            await db.Clothes.Include(c => c.Sizes)
                .Where(c => c.Name.Contains(name))
                .ToListAsync());

        // Изменить количество товара определённого размера — только менеджер
        app.MapPatch("/api/sizes/{id}/quantity", async (int id, UpdateQuantityRequest req, AppDbContext db) =>
        {
            if (req.Quantity < 0)
                return Results.BadRequest("Количество не может быть отрицательным");

            var size = await db.ClothingSizes.FindAsync(id);
            if (size is null)
                return Results.NotFound("Размер не найден");

            size.Quantity = req.Quantity;
            await db.SaveChangesAsync();
            return Results.Ok(size);
        }).RequireAuthorization("Manager");

        // Убрать товар из продажи — только менеджер
        app.MapPatch("/api/clothes/{id}/remove-from-sale", (int id, AppDbContext db) =>
            SetAvailable(id, false, db)).RequireAuthorization("Manager");

        // Вернуть товар в продажу — только менеджер
        app.MapPatch("/api/clothes/{id}/return-to-sale", (int id, AppDbContext db) =>
            SetAvailable(id, true, db)).RequireAuthorization("Manager");
    }

    private static async Task<IResult> SetAvailable(int id, bool available, AppDbContext db)
    {
        var clothing = await db.Clothes.FindAsync(id);
        if (clothing is null)
            return Results.NotFound("Товар не найден");

        clothing.IsAvailable = available;
        await db.SaveChangesAsync();
        return Results.Ok(clothing);
    }
}
