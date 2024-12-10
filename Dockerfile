# Используем базовый образ с .NET 8.0
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Билд-образ с SDK для .NET 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем .csproj файл
COPY ["Trenning_NotificationsExample/Trenning_NotificationsExample.csproj", "Trenning_NotificationsExample/"]
RUN dotnet restore "Trenning_NotificationsExample/Trenning_NotificationsExample.csproj"

# Копируем все файлы проекта
COPY . .
WORKDIR "/src/Trenning_NotificationsExample"
RUN dotnet build -c Release -o /app/build

# Публикуем проект
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Trenning_NotificationsExample.dll"]
