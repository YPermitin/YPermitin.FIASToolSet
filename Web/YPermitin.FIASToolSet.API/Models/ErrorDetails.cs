using System.Text.Json;

namespace YPermitin.FIASToolSet.API.Models
{
    /// <summary>
    /// Информация об ошибке
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// Статус сообщения
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
