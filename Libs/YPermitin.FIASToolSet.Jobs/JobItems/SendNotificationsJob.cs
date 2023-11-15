using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using YPermitin.FIASToolSet.Storage.Core.Models.Notifications;
using YPermitin.FIASToolSet.Storage.Core.Models.Versions;
using YPermitin.FIASToolSet.Storage.Core.Services;

namespace YPermitin.FIASToolSet.Jobs.JobItems
{
    /// <summary>
    /// Задание отправки уведомлений
    /// </summary>
    [DisallowConcurrentExecution]
    public sealed class SendNotificationsJob : IJob
    {
        private readonly ILogger<SendNotificationsJob> _logger;
        private readonly IServiceProvider _provider;
        private readonly IConfiguration _configuration;

        public SendNotificationsJob(
            IServiceProvider provider,
            ILogger<SendNotificationsJob> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _provider = provider;
            _configuration = configuration;
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Запущена отправка сообщений.");

            string telegramBotToken = _configuration.GetValue("Jobs:TelegramBotToken", string.Empty);
            string telegramChatId = _configuration.GetValue("Jobs:TelegramChatId", string.Empty);
            if (string.IsNullOrEmpty(telegramBotToken) || string.IsNullOrEmpty(telegramChatId))
            {
                _logger.LogError("Отправка уведомлений прервана. Не заданы настройки (токен бота и/или идентификатор чата).");
                return;
            }
            
            using (var scope = _provider.CreateScope())
            {
                IFIASMaintenanceRepository fiasMaintenanceService = scope.ServiceProvider.GetRequiredService<IFIASMaintenanceRepository>();

                var notificationsToSent = await fiasMaintenanceService.GetNotifications(NotificationStatus.Added);

                if (notificationsToSent.Any())
                {
                    var botClient = new TelegramBotClient(telegramBotToken);

                    foreach (var notificationItem in notificationsToSent)
                    {
                        await fiasMaintenanceService.BeginTransactionAsync();

                        try
                        {
                            if (notificationItem.NotificationTypeId == NotificationType.NewVersionOfFIAS)
                            {
                                FIASVersion lastVersion;
                                if (notificationItem.FIASVersionId == null)
                                    lastVersion = await fiasMaintenanceService.GetLastVersion();
                                else
                                    lastVersion = await fiasMaintenanceService.GetVersion((Guid)notificationItem.FIASVersionId);

                                FIASVersion previousVersion;
                                if (lastVersion != null)
                                    previousVersion = await fiasMaintenanceService.GetPreviousVersion(lastVersion.Id);
                                else
                                    previousVersion = null;

                                StringBuilder messageBuilder = new StringBuilder();

                                if (previousVersion == null // Нет данных о предыдущей версии
                                    // ИЛИ отличается информация о версии (ID, описание, дата)
                                    || (lastVersion.VersionId != previousVersion.VersionId
                                        && lastVersion.TextVersion != previousVersion.TextVersion
                                        && lastVersion.Date != previousVersion.Date))
                                {
                                    messageBuilder.AppendLine("*Обнаружена новая версия ФИАС!*");
                                    messageBuilder.AppendLine();
                                    messageBuilder.AppendLine($"Имя: *{lastVersion.TextVersion}*");
                                    messageBuilder.AppendLine($"Версия: *{lastVersion.VersionId}*");
                                    messageBuilder.AppendLine($"Дата: *{lastVersion.Date:dd.MM.yyyy}*");
                                    messageBuilder.AppendLine();
                                } else if (lastVersion.VersionId != previousVersion.VersionId
                                    || lastVersion.TextVersion != previousVersion.TextVersion
                                    || lastVersion.Date != previousVersion.Date) 
                                    // Изменилось одно из значений определяющих версию
                                {
                                    messageBuilder.AppendLine("*Обнаружено изменение в версии ФИАС!*");
                                    messageBuilder.AppendLine();
                                    
                                    if(lastVersion.TextVersion != previousVersion.TextVersion)
                                        messageBuilder.AppendLine($"Имя: *{lastVersion.TextVersion}*");
                                    if (lastVersion.VersionId != previousVersion.VersionId)
                                        messageBuilder.AppendLine($"Версия: *{lastVersion.VersionId}*");
                                    if(lastVersion.Date != previousVersion.Date)
                                        messageBuilder.AppendLine($"Дата: *{lastVersion.Date:dd.MM.yyyy}*");

                                    messageBuilder.AppendLine();
                                }
                                else // В остальных случаях считаем, что обновлена информация, связанная с версией
                                {
                                    messageBuilder.AppendLine("*Обнаружено изменение в версии ФИАС!*");
                                    messageBuilder.AppendLine();
                                }

                                // Проверка есть ли изменившиеся ссылки
                                bool linksChangedTitleWasAdded = false;
                                List<InlineKeyboardButton> urls = new();
                                if (lastVersion?.GARFIASXmlComplete != previousVersion?.GARFIASXmlComplete)
                                {
                                    if (lastVersion.GARFIASXmlComplete != null)
                                    {
                                        AddLinksChangedTitleWasAdded(messageBuilder, ref linksChangedTitleWasAdded);
                                        messageBuilder.AppendLine("- Изменилась ссылка на полную базу ГАР ФИАС.");
                                        urls.Add(InlineKeyboardButton.WithUrl(
                                            "ПОЛНАЯ",
                                            lastVersion.GARFIASXmlComplete));
                                    } else if (lastVersion?.GARFIASXmlComplete == null && previousVersion?.GARFIASXmlComplete != null)
                                    {
                                        messageBuilder.AppendLine("- Удалена ссылка на полную базу ГАР ФИАС.");
                                    }
                                }
                                if (lastVersion?.GARFIASXmlDelta != previousVersion?.GARFIASXmlDelta)
                                {
                                    if (lastVersion.GARFIASXmlDelta != null)
                                    {
                                        AddLinksChangedTitleWasAdded(messageBuilder, ref linksChangedTitleWasAdded);
                                        messageBuilder.AppendLine("- Изменилась ссылка на изменения в ГАР ФИАС.");
                                        urls.Add(InlineKeyboardButton.WithUrl(
                                            "ИЗМЕНЕНИЯ",
                                            lastVersion.GARFIASXmlDelta));
                                    }
                                    else if (lastVersion?.GARFIASXmlDelta == null && previousVersion?.GARFIASXmlDelta != null)
                                    {
                                        messageBuilder.AppendLine("- Удалена ссылка на изменения в ГАР ФИАС.");
                                    }
                                }
                                if (lastVersion?.KLADR47zComplete != previousVersion?.KLADR47zComplete)
                                {
                                    if (lastVersion.KLADR47zComplete != null)
                                    {
                                        AddLinksChangedTitleWasAdded(messageBuilder, ref linksChangedTitleWasAdded);
                                        messageBuilder.AppendLine("- Изменилась ссылка на КЛАДР 4 (7z).");
                                        urls.Add(InlineKeyboardButton.WithUrl(
                                            "КЛАДР 4 7z",
                                            lastVersion.KLADR47zComplete));
                                    }
                                    else if (lastVersion?.KLADR47zComplete == null && previousVersion?.KLADR47zComplete != null)
                                    {
                                        messageBuilder.AppendLine("- Удалена ссылка на КЛАДР 4 (7z).");
                                    }
                                }
                                if (lastVersion?.KLADR4ArjComplete != previousVersion?.KLADR4ArjComplete)
                                {
                                    if (lastVersion.KLADR4ArjComplete != null)
                                    {
                                        AddLinksChangedTitleWasAdded(messageBuilder, ref linksChangedTitleWasAdded);
                                        messageBuilder.AppendLine("- Изменилась ссылка на КЛАДР 4 (Arj).");
                                        urls.Add(InlineKeyboardButton.WithUrl(
                                            "КЛАДР 4 ARJ",
                                            lastVersion.KLADR4ArjComplete));
                                    }
                                    else if (lastVersion?.KLADR4ArjComplete == null && previousVersion?.KLADR4ArjComplete != null)
                                    {
                                        messageBuilder.AppendLine("- Удалена ссылка на КЛАДР 4 (Arj).");
                                    }
                                }

                                string message = messageBuilder.ToString();
                                message = message.Replace("!", "\\!");
                                message = message.Replace(".", "\\.");
                                message = message.Replace("(", "\\(");
                                message = message.Replace(")", "\\)");
                                message = message.Replace("-", "\\-");

                                var replyMarkup = new InlineKeyboardMarkup(urls);

                                await botClient.SendTextMessageAsync(
                                    chatId: telegramChatId,
                                    text: message,
                                    parseMode: ParseMode.MarkdownV2,
                                    replyMarkup: replyMarkup,
                                    cancellationToken: CancellationToken.None);
                            }
                            else if (notificationItem.NotificationTypeId == NotificationType.Custom)
                            {
                                await botClient.SendTextMessageAsync(
                                    chatId: telegramChatId,
                                    text: notificationItem.Content,
                                    parseMode: ParseMode.MarkdownV2,
                                    cancellationToken: CancellationToken.None);
                            }
                            
                            notificationItem.StatusId = NotificationStatus.Sent;
                            _logger.LogInformation($"Сообщение {notificationItem.Id}: успешно отправлено.");
                        }
                        catch (Exception ex)
                        {
                            notificationItem.StatusId = NotificationStatus.Canceled;
                            _logger.LogError($"Ошибка при отправке сообщения {notificationItem.Id}: {ex}");
                        }

                        fiasMaintenanceService.UpdateNotification(notificationItem);
                        await fiasMaintenanceService.SaveAsync();

                        await fiasMaintenanceService.CommitTransactionAsync();
                    }
                }
            }

            _logger.LogInformation("Завершена отправка сообщения.");
        }

        private void AddLinksChangedTitleWasAdded(StringBuilder messageBuilder, ref bool linksChangedTitleWasAdded)
        {
            if (!linksChangedTitleWasAdded)
            {
                messageBuilder.AppendLine("Изменены ссылки для скачивания:");

                linksChangedTitleWasAdded = true;
            }
        }
    }
}
