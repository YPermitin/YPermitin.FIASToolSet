namespace YPermitin.FIASToolSet.Storage.Core.Models
{
    /// <summary>
    /// Информация о версии ФИАС
    /// </summary>
    public class FIASVersion
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Период добавления записи о версии
        /// </summary>
        public DateTime Period { get; set; }

        /// <summary>
        /// Идентификатор версии (в прямых выгрузках дата выгрузки вида yyyyMMdd)
        /// </summary>
        public int VersionId { get; set; }

        /// <summary>
        /// Описание версии файла в текстовом виде
        /// </summary>
        public string TextVersion { get; set; }

        /// <summary>
        /// Дата выгрузки (dd.MM.yyyy)
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Файлы дистрибутива ФИАС в формате DBF (полная база)
        /// </summary>
        public string FIASDbfComplete { get; set; }

        /// <summary>
        /// Файлы дистрибутива ФИАС в формате DBF (изменения)
        /// </summary>
        public string FIASDbfDelta { get; set; }

        /// <summary>
        /// Файлы дистрибутива ФИАС в формате XML (полная база)
        /// </summary>
        public string FIASXmlComplete { get; set; }

        /// <summary>
        /// Файлы дистрибутива ФИАС в формате XML (изменения)
        /// </summary>
        public string FIASXmlDelta { get; set; }

        /// <summary>
        /// Файлы дистрибутива ГАР ФИАС в формате XML (полная база)
        /// </summary>
        public string GARFIASXmlComplete { get; set; }

        /// <summary>
        /// Файлы дистрибутива ГАР ФИАС в формате XML (изменения)
        /// </summary>
        public string GARFIASXmlDelta { get; set; }

        /// <summary>
        /// Файлы дистрибутива КЛАДР 4 в формате ARJ (полная база)
        /// </summary>
        public string KLADR4ArjComplete { get; set; }

        /// <summary>
        /// Файлы дистрибутива КЛАДР 4 в формате 7Z (полная база)
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string KLADR47zComplete { get; set; }
    }
}
