namespace YPermitin.FIASToolSet.Jobs.Initializer
{
    /// <summary>
    /// Команды управления заданиями
    /// </summary>
    public enum CommandState
    {
        /// <summary>
        /// Первый запуск
        /// </summary>
        InitStart = -1,

        /// <summary>
        /// Действий не требуется
        /// </summary>
        NoAction = 0,

        /// <summary>
        /// Перезапуск
        /// </summary>
        Restart = 1,

        /// <summary>
        /// Остановка
        /// </summary>
        Stop = 2
    }
}
