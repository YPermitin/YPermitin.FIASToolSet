using System.Collections.Generic;
using System.Threading.Tasks;
using YPermitin.FIASToolSet.Loader.Models;

namespace YPermitin.FIASToolSet.Loader
{
    /// <summary>
    /// Объект работы с дистрибутивами ФИАС разных форматов и типов
    /// </summary>
    public interface IFIASLoader
    {
        /// <summary>
        /// Информацию о последней версии файлов дистрибутива, доступных для скачивания
        /// </summary>
        /// <returns>Дистрибутив классификатора ФИАС</returns>
        Task<FIASDistributionInfo> GetLastDistributionInfo();

        /// <summary>
        /// Информацию обо всех версиях файлов дистрибутивов, доступных для скачивания
        /// </summary>
        /// <returns>Коллекция доступных дистрибутивов классификатора ФИАС</returns>
        Task<IReadOnlyList<FIASDistributionInfo>> GetAllDistributionInfo();
    }
}
