# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Users/Users.API/Users.API.csproj", "Users/Users.API/"]
COPY ["Users/Users.Application/Users.Application.csproj", "Users/Users.Application/"]
COPY ["Users/Users.Infrastructure/Users.Infrastructure.csproj", "Users/Users.Infrastructure/"]
COPY ["Users/Users.Domain/Users.Domain.csproj", "Users/Users.Domain/"]
COPY ["Shared/Shared/Shared.csproj", "Shared/Shared/"]
RUN dotnet restore "Users/Users.API/Users.API.csproj"
COPY . .
WORKDIR "/src/Users/Users.API"
RUN dotnet build "./Users.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Users.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Users.API.dll"]