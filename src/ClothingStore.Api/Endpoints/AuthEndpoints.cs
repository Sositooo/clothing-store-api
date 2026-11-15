using ClothingStore.Api.Data;
using ClothingStore.Api.Models;
using ClothingStore.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Api.Endpoints;

public record RegisterRequest(string Login, string Password, string Phone, string DeliveryAddress);
public record LoginRequest(string Login, string Password);

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        // Регистрация — может любой, всегда создаётся обычный пользователь
        app.MapPost("/api/auth/register", async (RegisterRequest req, AppDbContext db, PasswordHasher hasher) =>
        {
            if (string.IsNullOrWhiteSpace(req.Login) || string.IsNullOrWhiteSpace(req.Password) ||
                string.IsNullOrWhiteSpace(req.Phone) || string.IsNullOrWhiteSpace(req.DeliveryAddress))
                return Results.BadRequest("Заполните логин, пароль, телефон и адрес доставки");

            if (await db.Users.AnyAsync(u => u.Login == req.Login))
                return Results.Conflict("Пользователь с таким логином уже существует");

            var user = new User
            {
                Login = req.Login,
                PasswordHash = hasher.Hash(req.Password),
                Phone = req.Phone,
                DeliveryAddress = req.DeliveryAddress,
                Role = UserRole.User
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();

            return Results.Ok(user);
        });

        // Авторизация — возвращает JWT-токен
        app.MapPost("/api/auth/login", async (LoginRequest req, AppDbContext db, PasswordHasher hasher, TokenService tokens) =>
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Login == req.Login);
            if (user is null || !hasher.Verify(req.Password, user.PasswordHash))
                return Results.Unauthorized();

            return Results.Ok(new { token = tokens.Create(user), user });
        });
    }
}
