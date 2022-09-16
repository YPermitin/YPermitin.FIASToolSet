namespace YPermitin.FIASToolSet.Jobs.Initializer
{
    /// <summary>
    /// Менеджер управления командами состояния заданий
    /// </summary>
    public class CommandStateManager : ICommandStateManager
    {
        private readonly object _commandStateObject = new();
        private CommandState _commandState;

        public CommandStateManager()
        {
            _commandState = CommandState.InitStart;
        }

        /// <summary>
        /// Установить команду "Действйи не требуется"
        /// </summary>
        public void SetNoAction()
        {
            lock (_commandStateObject)
            {
                _commandState = CommandState.NoAction;
            }
        }

        /// <summary>
        /// Установить команду "Перезапуск"
        /// </summary>
        public void SetRestart()
        {
            lock (_commandStateObject)
            {
                _commandState = CommandState.Restart;
            }
        }

        /// <summary>
        /// Установить команду "Остановить задания"
        /// </summary>
        public void SetStop()
        {
            lock (_commandStateObject)
            {
                _commandState = CommandState.Stop;
            }
        }

        /// <summary>
        /// Получение текущей команды управления заданиями и бросить ее на "Действйи не требуется"
        /// </summary>
        /// <returns>Команда управления заданиями</returns>
        public CommandState GetLastActionAndReset()
        {
            CommandState lastAction;

            lock (_commandStateObject)
            {
                lastAction = _commandState;
                SetNoAction();
            }

            return lastAction;
        }
    }
}
