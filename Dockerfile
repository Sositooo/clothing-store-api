# ---- build ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY src/ClothingStore.Api/ClothingStore.Api.csproj src/ClothingStore.Api/
RUN dotnet restore src/ClothingStore.Api/ClothingStore.Api.csproj
COPY . .
RUN dotnet publish src/ClothingStore.Api/ClothingStore.Api.csproj -c Release -o /app/publish --no-restore

# ---- runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "ClothingStore.Api.dll"]
