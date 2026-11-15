# Clothing Store API — ПР №3

API для магазина одежды: ASP.NET Core 9 + Microsoft SQL Server + JWT.

## Как запустить

1. Установить: .NET 9 SDK, SQL Server (Express или Developer) + SSMS, Postman.
2. Проверить строку подключения в `src/ClothingStore.Api/appsettings.json` (`ConnectionStrings:Default`):
   - SQL Server по умолчанию: `Server=localhost;...`
   - SQL Server **Express**: `Server=localhost\\SQLEXPRESS;...`
   - вход по логину/паролю вместо Windows: `Server=localhost;Database=ClothingStore;User Id=sa;Password=ВАШ_ПАРОЛЬ;TrustServerCertificate=True`
3. Запустить:
   ```
   cd src/ClothingStore.Api
   dotnet run
   ```
   База `ClothingStore`, таблицы, менеджер и товары создадутся сами. API: http://localhost:5000
   В SSMS база появится в Object Explorer → Databases → ClothingStore (нажмите F5, чтобы обновить).
4. Postman → Import → `postman/ClothingStore.postman_collection.json` → выполнить 11 запросов по порядку (как в задании).

Менеджер: `manager` / `Manager123!`

## Структура

```
src/ClothingStore.Api/
├── Models/      Clothing, ClothingSize, User, Order, OrderItem, enum'ы
├── Data/        AppDbContext, DbSeeder (начальные данные), Migrations
├── Services/    PasswordHasher (BCrypt), TokenService (JWT)
├── Endpoints/   AuthEndpoints, ClothesEndpoints, OrdersEndpoints
└── Program.cs
```

## Запросы

| Метод | URL | Доступ |
|---|---|---|
| POST | /api/auth/register | все |
| POST | /api/auth/login | все |
| GET | /api/clothes | все |
| GET | /api/clothes/search?name=футб | все |
| POST | /api/orders | авторизованный |
| GET | /api/orders?status=Created | авторизованный |
| GET | /api/orders/user/{userId} | авторизованный |
| PATCH | /api/orders/{id}/status | менеджер |
| PATCH | /api/sizes/{id}/quantity | менеджер |
| PATCH | /api/clothes/{id}/remove-from-sale | менеджер |
| PATCH | /api/clothes/{id}/return-to-sale | менеджер |

Статусы заказа: `Created`, `Paid`, `Shipped`, `Delivered`, `Cancelled`.
