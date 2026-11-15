# Clothing Store API — ПР №3

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
