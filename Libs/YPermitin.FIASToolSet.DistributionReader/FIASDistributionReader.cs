using YPermitin.FIASToolSet.DistributionReader.DataReaders;
using YPermitin.FIASToolSet.DistributionReader.Exceptions;
using YPermitin.FIASToolSet.DistributionReader.Models;

namespace YPermitin.FIASToolSet.DistributionReader;

public class FIASDistributionReader
{
    private readonly string _workingDirectory;
    
    public FIASDistributionReader(string workingDirectory)
    {
        _workingDirectory = workingDirectory;
    }

    /// <summary>
    /// Получение версии дистрибутива ФИАС в строковм виде
    /// </summary>
    /// <returns>Строка с информацией о версии ФИАС</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось найти файл с информацией о версии</exception>
    public string GetVersionAsString()
    {
        string fileVersionInfoPath = Path.Combine(
            _workingDirectory,
            "version.txt"
        );
        
        string version;

        if (File.Exists(fileVersionInfoPath))
        {
            version = File.ReadAllText(fileVersionInfoPath).Trim();
        }
        else
        {
            throw new FIASDataNotFoundException("Не обнаружен файл с информацией о версии.", fileVersionInfoPath);
        }

        return version;
    }

    /// <summary>
    /// Получение версии дистрибутива ФИАС в числовом виде
    /// </summary>
    /// <returns>Версия ФИАС числом</returns>
    /// <exception cref="FIASBadDataException">Не удалось конвертировать значение версии ФИАС в число</exception>
    public int GetVersion()
    {
        string versionAsString = GetVersionAsString();

        if (int.TryParse(versionAsString.Replace(".", string.Empty), out int versionAsInt))
        {
            return versionAsInt;
        }
        else
        {
            var exceptionObject = new FIASBadDataException(
                $"Не удалось конвертировать значение версии \"{versionAsString}\" в числовой формат.");
            exceptionObject.Data.Add("VersionAsString", versionAsString);
            throw exceptionObject;
        }
    }

    public NormativeDocKindReader GetNormativeDocKinds()
    {
        string fileNormativeDocKindsPath;
        var foundFiles = Directory.GetFiles(_workingDirectory, "AS_NORMATIVE_DOCS_KINDS_20230706_*.XML");
        if (foundFiles.Length == 1)
        {
            FileInfo foundFileInfo = new FileInfo(foundFiles[0]);
            fileNormativeDocKindsPath = Path.Combine(
                _workingDirectory,
                foundFileInfo.Name
            );
        }
        else
        {
            throw new FIASDataNotFoundException(
                "Не обнаружен файл с информацией о видах нормативных документов.",
                "AS_NORMATIVE_DOCS_KINDS_20230706_*.XML");
        }

        return new NormativeDocKindReader(fileNormativeDocKindsPath);
    }
}