# ПО "Каталог продуктов"

Backend тестового задания на C# / .NET.

Проект состоит из двух микросервисов:

* **Users** - аутентификация и управление пользователями.
* **Products** - управление категориями и продуктами.

## Запуск

Требуется установленный **Docker** с поддержкой Docker Compose.

Откройте `cmd` и перейдите в директорию `backend`:

Запустите проект:

```cmd
.\docker-compose.ps1 up -b
```

После запуска API доступны по адресам:

* Users: http://localhost:8080/scalar
* Products: http://localhost:8082/scalar

**Важно:** после запуска необходимо дождаться полной инициализации сервисов.

При запуске микросервисы ожидают готовности своих баз данных и выполняют миграции и инициализацию. Пока база данных не готова, API может перезапускаться - это нормальное поведение.

## Остановка

```cmd
.\docker-compose.ps1 down
```

Для остановки и удаления данных:

```cmd
.\docker-compose.ps1 down -v
```

## Используемые технологии

* C#
* .NET 10
* ASP.NET Core
* Entity Framework Core
* Microsoft SQL Server
* JWT
* gRPC
* Docker / Docker Compose
* MediatR
* FluentValidation
* AutoMapper
* QuestPDF
* Scalar
* Clean Architecture
