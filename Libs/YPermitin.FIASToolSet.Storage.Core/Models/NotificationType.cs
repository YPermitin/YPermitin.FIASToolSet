namespace YPermitin.FIASToolSet.Storage.Core.Models
{
    /// <summary>
    /// Тип уведомления
    /// </summary>
    public class NotificationType
    {
        /// <summary>
        /// Уведомление об обнаружении новой версии ФИАС
        /// </summary>
        public static readonly Guid NewVersionOfFIAS = new("50be368c-0f06-483a-a5b8-2de9113a4f27");

        /// <summary>
        /// Произвольное уведомление
        /// </summary>
        public static readonly Guid Custom = new("749041e9-f51d-48b7-abe0-14ba50436431");

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
