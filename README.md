# YPermitin.FIASToolSet

Набор инструментов для работы с [Федеральной Информационной Адресной Системой (ФИАС)](https://fias.nalog.ru/).

## Обратная связь и новости

Вопросы, предложения и любую другую информацию [отправляйте на электронную почту](mailto:i.need.ypermitin@yandex.ru).

Новости по проектам или новым материалам в [Telegram-канале](https://t.me/TinyDevVault).

## Функциональность

В текущей версии сервис позволяет:

* Открытое API сервиса:
	* Получение информации о текущей версии ФИАС по данным ФНС.
	* Получение информации о текущей версии ФИАС по данным сервиса.
* Задание актуализации / отслеживания версии ФИАС по данным ФНС.
* [Отправка уведомлений о событиях сервиса в Telegram](https://t.me/tinydevtools_fias):
	* Появление новой версии ФИАС.

Варианты публикации сервиса:

* Windows (на базе IIS или в качестве службы, а также через веб-сервер Kestrel)
* *.nix (на базе Kestrel + реверс-прокси через Apache/Nginx)

## Окружение для разработки

Для окружение разработчика необходимы:

* [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
* [Visual Studio 2022](https://visualstudio.microsoft.com/ru/vs/)
* [PostgreSQL 12/13/14](https://www.postgresql.org/download/) / [SQL Server 2012+](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

## Состав проекта

Проекты и библиотеки в составе решения:

* Web - проекты API или других видов веб-приложений.
	* YPermitin.FIASToolSet.API - API для работы с классификатором ФИАС.
* Libs - библиотеки и вспомогательные проекты.
	* YPermitin.FIASToolSet.DistributionBrowser - библиотека для работы с дистрибутивами ФИАС с официального сайта (загрузка основного дистрибутива и пакетов обновления, проверка версий).
	* YPermitin.FIASToolSet.Jobs - проект заданий для отслеживания обновлений ФИАС, загрузки базы и ее обновления, а также некоторых других действий для обслуживнаия.
	* YPermitin.FIASToolSet.Storage.Core - базовый проект для работы с базой данных сервиса.
	* YPermitin.FIASToolSet.Storage.PostgreSQL - проект для работы с базой данных сервиса в PostgreSQL.
    * YPermitin.FIASToolSet.Storage.SQLServer - проект для работы с базой данных сервиса в Microsoft SQL Server.
* Tests - модульные тесты и связанные проверки.
	* YPermitin.FIASToolSet.DistributionBrowser.Tests - тесты библиотеки для работы с дистрибутивами ФИАС.
	* YPermitin.FIASToolSet.API.Tests - тесты приложения API для работы с классификатором ФИАС.

## Развертывание проекта

Установите ASP.NET Core Runtime 9 и SQL Server / PostgreSQL (12+), после чего опубликуйте приложение "YPermitin.FIASToolSet.API". Подробнее о публикации ASP.NET Core приложений читайте в [официальной документации](https://docs.microsoft.com/ru-ru/aspnet/core/host-and-deploy/?view=aspnetcore-6.0).

### appsettings.json

Файл настроек приложения "appsettings.json" сервиса:

```json
{
  "DeployType": "Kestrel",
  "DBMSType": "SQLServer",
  "ConnectionStrings": {
    "FIASToolSetService": "User ID=<user>;Password=<password>;Host=<server>;Port=5432;Database=FIASToolSetService;"
  },
  "Serilog": {
    "MinimumLevel": "Debug"
  },
  "AllowedHosts": "*",
  "Jobs": {
    "Schedules": {
      "ActualizeFIASVersionHistoryJob": "0 0/10 * * * ?",
      "SendNotificationsJob": "0 0/1 * * * ?"
    },
    "MaxBatchSize": 10,
    "ThreadPoolConcurrency": 10,
    "EnableNotification": true,
    "TelegramBotToken": "-",
    "TelegramChatId": "-"
  },
  "CORS": {
    "AllowedOrigins": [
    ]
  }
}

```

В параметре *"DeployType"* устанавливается тип публикации сервиса:

* *Kestrel* - для использования веб-сервера Kestrel. Значение по умолчанию для *.nix-систем.
* *IIS* - для использования IIS. Значение по умолчанию для Windows-систем.
* *WindowsService* - публикация в качестве службы Windows.

Строка подключения к базе данных сервиса хранится в настройке *"ConnectionStrings:FIASToolSetService"*. Ее формат зависит от используемой СУБД, которая устанавливается в параметре *"DBMSType"*. Доступные значения:

* PostgreSQL (значение по умолчанию)
* SQLServer

Раздел настройки заданий *"Jobs"*:

* *"MaxBatchSize"* - максимальное количество одновременно выполняемых заданий. Можно не настраивать, тогда будет использоваться значение по умолчанию - 10.
* *"ThreadPoolConcurrency"* - размер пула потоков планировщика заданий. Можно не настраивать, тогда будет использоваться значение по умолчанию - 10.
* *"EnableNotification"* - включение механизма уведомлений. Если включено, то будет выполняться регистрация событий уведомления и при старте приложения будет запущено задание отправки уведомлений.
* *"TelegramBotToken"* - токен Telegram-бота для отправки уведомлений о событиях работы сервиса.
* *"TelegramChatId"* - идентификатор чата Telegram, в котором учавствует бот и куда будут отправлены уведомления.
* *"Schedules"* - расписание в формате CRON для каждогого задания.

Настройка *"CORS:AllowedOrigins"* применяется для настройки политики CORS, чтобы разрешить определённым сайтам делать запросы со стороны клиента к API.

При необходимости переопределить порт Kestrel можно указать штатный раздел настроек:

```json
"Kestrel": {
   "Endpoints": {
      "Http": {
        "Url": "http://localhost:5100"
      }
    }
}
```

В разделе "Serilog" находится настройка логирования. Нужно установить минимальный уровень логирования событий. Для рабочего окружения рекомендую только ошибки (Error), для разработки оставить уровень отладки (Debug).

## Планы на будущее

* Использование TimescaleDB для PostgreSQL (в т.ч. использование сжатия и обновление данных в сжатих секциях).
* Добавить возможность хранения классификатора ФИАС в ClickHouse.
* Реализовать API для работы с данными ФИАС.
* Сделать пример использования API в [веб-приложении TinyDevTools](https://tinydevtools.ru/fias).

## Лицензия, благодарности и послесловие

Проект делается на чистом интересе и не преследует коммерческих целей. 

Публикуется под лицензией MIT, поэтому Вы можете использовать его у себя полностью или частично без каких-либо гарантий и полностью под Вашу ответственность.

Спасибо [ФНС за открытые данные базы реестра адресов - ФИАС](https://fias.nalog.ru/).
