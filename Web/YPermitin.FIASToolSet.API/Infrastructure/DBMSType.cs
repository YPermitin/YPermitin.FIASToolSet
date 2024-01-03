namespace YPermitin.FIASToolSet.API.Infrastructure
{
    /// <summary>
    /// Тип используемой СУБД
    /// </summary>
    public enum DBMSType
    {
        /// <summary>
        /// PostgreSQL
        /// </summary>
        PostgreSQL,

        /// <summary>
        /// SQLServer
        /// </summary>
        SQLServer,
        
        /// <summary>
        /// ClickHouse
        /// </summary>
        ClickHouse,

        /// <summary>
        /// Неизвестный тип
        /// </summary>
        Unknown
    }
}
