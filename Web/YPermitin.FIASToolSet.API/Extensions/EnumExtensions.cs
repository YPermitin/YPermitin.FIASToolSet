namespace YPermitin.FIASToolSet.API.Extensions
{
    /// <summary>
    /// Вспомогательные методы работы с перечислениями
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Преобразовать строку в значение перечисления
        /// </summary>
        /// <typeparam name="T">Тип перечисления</typeparam>
        /// <param name="value">Строковое значение для преобразования</param>
        /// <param name="defaultValue">Значение перечисления по умолчанию</param>
        /// <returns>Значение перечисления после преобразования</returns>
        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return Enum.TryParse<T>(value, true, out var result) ? result : defaultValue;
        }
    }
}
