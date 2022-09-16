using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using YPermitin.FIASToolSet.Storage.Core.Models;
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
                                var lastVersion = await fiasMaintenanceService.GetLastVersion();
                                string message =
                                    "*Обнаружена новая версия ФИАС!*\n" +
                                    "\n" +
                                    $"Имя: *{lastVersion.TextVersion}*\n" +
                                    $"Версия: *{lastVersion.VersionId}*\n" +
                                    $"Дата: *{lastVersion.Date:dd.MM.yyyy}*\n" +
                                    $"\n" +
                                    $"Ссылки для скачивания базы ниже.";
                                message = message.Replace("!", "\\!");
                                message = message.Replace(".", "\\.");
                                message = message.Replace("(", "\\(");
                                message = message.Replace(")", "\\)");

                                List<InlineKeyboardButton> urls = new();
                                if (lastVersion.GARFIASXmlComplete != null)
                                    urls.Add(InlineKeyboardButton.WithUrl(
                                        "ПОЛНАЯ",
                                        lastVersion.GARFIASXmlComplete));
                                if (lastVersion.GARFIASXmlDelta != null)
                                    urls.Add(InlineKeyboardButton.WithUrl(
                                        "ИЗМЕНЕНИЯ",
                                        lastVersion.GARFIASXmlDelta));
                                if (lastVersion.KLADR47zComplete != null)
                                    urls.Add(InlineKeyboardButton.WithUrl(
                                        "КЛАДР 4 7z",
                                        lastVersion.KLADR47zComplete));
                                if (lastVersion.KLADR4ArjComplete != null)
                                    urls.Add(InlineKeyboardButton.WithUrl(
                                        "КЛАДР 4 ARJ",
                                        lastVersion.KLADR4ArjComplete));

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
    }
}
