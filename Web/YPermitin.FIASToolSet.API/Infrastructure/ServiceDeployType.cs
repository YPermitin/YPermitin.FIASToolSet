namespace YPermitin.FIASToolSet.API.Infrastructure
{
    /// <summary>
    /// Тип публикации приложения
    /// </summary>
    public enum ServiceDeployType
    {
        /// <summary>
        /// На веб-сервере IIS (Internet Information Services)
        /// </summary>
        IIS,

        /// <summary>
        /// На веб-сервере Kestrel
        /// </summary>
        Kestrel,

        /// <summary>
        /// Служба Windows
        /// </summary>
        WindowsService,

        /// <summary>
        /// Неизвестный тип
        /// </summary>
        Unknown
    }
}
