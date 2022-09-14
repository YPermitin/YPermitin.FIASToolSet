using System.Collections.Generic;
using System.Threading.Tasks;
using YPermitin.FIASToolSet.DistributionBrowser.Models;

namespace YPermitin.FIASToolSet.DistributionBrowser
{
    /// <summary>
    /// Объект работы с дистрибутивами ФИАС разных форматов и типов
    /// </summary>
    public interface IFIASDistributionBrowser
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
