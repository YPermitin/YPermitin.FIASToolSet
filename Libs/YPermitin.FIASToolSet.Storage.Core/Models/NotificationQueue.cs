using System.ComponentModel.DataAnnotations.Schema;

namespace YPermitin.FIASToolSet.Storage.Core.Models
{
    /// <summary>
    /// Элемент уведомления в общей очереди
    /// </summary>
    public class NotificationQueue
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Период добавления уведомления
        /// </summary>
        public DateTime Period { get; set; }

        /// <summary>
        /// Идентификатор состояния уведомления
        /// </summary>        
        public Guid StatusId { get; set; }

        /// <summary>
        /// Состояние уведомления
        /// </summary>
        [ForeignKey("StatusId")]
        public NotificationStatus Status { get; set; }

        /// <summary>
        /// Идентификатор типа уведомления
        /// </summary>        
        public Guid NotificationTypeId { get; set; }

        /// <summary>
        /// Тип уведомления
        /// </summary>
        [ForeignKey("NotificationTypeId")]
        public NotificationType NotificationType { get; set; }

        /// <summary>
        /// Идентификатор версии ФИАС
        /// </summary>
        public Guid? FIASVersionId { get; set; }

        /// <summary>
        /// Версия ФИАС
        /// </summary>
        [ForeignKey("FIASVersionId")]
        public FIASVersion FIASVersion { get; set; }

        /// <summary>
        /// Произвольные данные для сообщения
        /// </summary>
        public string Content { get; set; }
    }
}
