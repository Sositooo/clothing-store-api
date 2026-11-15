using System.Text;
using System.Text.Json.Serialization;
using ClothingStore.Api.Data;
using ClothingStore.Api.Endpoints;
using ClothingStore.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Как в методичке: игнорируем циклы (Order -> Items -> ... -> Order).
// enum'ы отдаём строками: "Created", "Manager"
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// База данных SQL Server (смотреть таблицы можно в SSMS)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddSingleton<TokenService>();

// Авторизация по JWT-токену
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };
    });

// Политика "Manager" — доступ только для менеджеров
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Manager", policy => policy.RequireRole("Manager"));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapClothesEndpoints();
app.MapOrdersEndpoints();

// Создаём таблицы и начальные данные (менеджер + товары)
await DbSeeder.MigrateAndSeedAsync(app.Services);

app.Run();
