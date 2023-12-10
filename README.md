# TreckTogether - Приложение для поиска попутчиков

TreckTogether - это бэкенд часть приложения, предназначенного для помощи людям в поиске водителей, которые могут взять пассажиров по пути.

## Описание проекта

Этот проект представляет собой ASP.NET Core Web API, разработанный на платформе .NET 6. База данных основана на MS SQL Server. Проект построен с учетом концепций RESTful API для обеспечения простого взаимодействия с клиентскими приложениями.

## Требования

Прежде чем начать работу с проектом, убедитесь, что у вас установлены следующие компоненты:

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [MS SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

## Установка и запуск

1. Склонируйте репозиторий:

   ```bash
   git clone https://github.com/Kubat555/TrekTogether.git

2. Создайте базу данных в MS SQL Server:

   ```bash
   dotnet ef database update

3. Запустите проект:

   ```bash
   dotnet run

## Документация API
Документация по использованию API доступна через Swagger