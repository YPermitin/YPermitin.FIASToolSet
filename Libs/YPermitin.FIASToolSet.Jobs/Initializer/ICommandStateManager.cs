namespace YPermitin.FIASToolSet.Jobs.Initializer
{
    /// <summary>
    /// Менеджер управления командами состояния заданий
    /// </summary>
    public interface ICommandStateManager
    {
        /// <summary>
        /// Установить команду "Действйи не требуется"
        /// </summary>
        void SetNoAction();

        /// <summary>
        /// Установить команду "Перезапуск"
        /// </summary>
        void SetRestart();

        /// <summary>
        /// Установить команду "Остановить задания"
        /// </summary>
        void SetStop();

        /// <summary>
        /// Получение текущей команды управления заданиями и бросить ее на "Действйи не требуется"
        /// </summary>
        /// <returns>Команда управления заданиями</returns>
        CommandState GetLastActionAndReset();
    }
}
