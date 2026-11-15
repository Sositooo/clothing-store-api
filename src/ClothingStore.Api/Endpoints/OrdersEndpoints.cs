using System.Security.Claims;
using ClothingStore.Api.Data;
using ClothingStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Api.Endpoints;

public record OrderItemRequest(int ClothingSizeId, int Quantity);
public record CreateOrderRequest(List<OrderItemRequest> Items);
public record UpdateStatusRequest(OrderStatus Status);

public static class OrdersEndpoints
{
    public static void MapOrdersEndpoints(this WebApplication app)
    {
        // Создать заказ — только авторизованный пользователь
        app.MapPost("/api/orders", async (CreateOrderRequest req, ClaimsPrincipal user, AppDbContext db) =>
        {
            if (req.Items is null || req.Items.Count == 0)
                return Results.BadRequest("Заказ должен содержать хотя бы один товар");

            var order = new Order
            {
                UserId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!),
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Created
            };

            foreach (var item in req.Items)
            {
                var size = await db.ClothingSizes.Include(s => s.Clothing)
                    .FirstOrDefaultAsync(s => s.Id == item.ClothingSizeId);

                if (size is null)
                    return Results.NotFound($"Размер с id {item.ClothingSizeId} не найден");
                if (item.Quantity <= 0)
                    return Results.BadRequest("Количество должно быть больше нуля");
                if (!size.Clothing.IsAvailable)
                    return Results.BadRequest($"Товар «{size.Clothing.Name}» снят с продажи");
                if (size.Quantity < item.Quantity)
                    return Results.BadRequest($"Недостаточно товара «{size.Clothing.Name}» размера {size.Letter}");

                size.Quantity -= item.Quantity;               // списываем со склада
                size.Clothing.PurchaseCount += item.Quantity; // увеличиваем количество покупок

                order.Items.Add(new OrderItem { ClothingSizeId = size.Id, Quantity = item.Quantity });
            }

            db.Orders.Add(order);
            await db.SaveChangesAsync(); // всё сохраняется одной транзакцией
            return Results.Ok(order);
        }).RequireAuthorization();

        // Заказы с определённым статусом — только авторизованный: /api/orders?status=Created
        app.MapGet("/api/orders", async (OrderStatus status, AppDbContext db) =>
            await OrdersQuery(db).Where(o => o.Status == status).ToListAsync())
            .RequireAuthorization();

        // Заказы определённого покупателя — только авторизованный
        app.MapGet("/api/orders/user/{userId}", async (int userId, AppDbContext db) =>
            await OrdersQuery(db).Where(o => o.UserId == userId).ToListAsync())
            .RequireAuthorization();

        // Изменить статус заказа — только менеджер
        app.MapPatch("/api/orders/{id}/status", async (int id, UpdateStatusRequest req, AppDbContext db) =>
        {
            var order = await db.Orders.FindAsync(id);
            if (order is null)
                return Results.NotFound("Заказ не найден");

            order.Status = req.Status;
            await db.SaveChangesAsync();
            return Results.Ok(order);
        }).RequireAuthorization("Manager");
    }

    // Заказ вместе с покупателем, позициями, размерами и товарами
    private static IQueryable<Order> OrdersQuery(AppDbContext db) => db.Orders
        .Include(o => o.User)
        .Include(o => o.Items).ThenInclude(i => i.ClothingSize).ThenInclude(s => s.Clothing)
        .OrderBy(o => o.Id);
}
