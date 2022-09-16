namespace YPermitin.FIASToolSet.Storage.Core.Models
{
    /// <summary>
    /// Состояние уведомления
    /// </summary>
    public class NotificationStatus
    {
        /// <summary>
        /// Статус "Добавлено"
        /// </summary>
        public static readonly Guid Added = new("fbb1221b-9a20-4672-b872-730810dbd7d5");

        /// <summary>
        /// Статус "Отправлено"
        /// </summary>
        public static readonly Guid Sent = new("f9ae7dcd-f55a-4810-8e96-62e1c0ad1923");

        /// <summary>
        /// Статус "Отменено"
        /// </summary>
        public static readonly Guid Canceled = new("7d3064ab-45fb-48c0-ac44-a91d1b2369b1");

        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
    }
}
