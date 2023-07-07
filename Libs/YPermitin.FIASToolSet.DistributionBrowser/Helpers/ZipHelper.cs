using System.IO;
using System.IO.Compression;

namespace YPermitin.FIASToolSet.DistributionBrowser.Helpers;

public static class ZipHelper
{
    public static bool IsZipValid(string path)
    {
        try
        {
            using (var zipFile = ZipFile.OpenRead(path))
            {
                var entries = zipFile.Entries;
                return true;
            }
        }
        catch (InvalidDataException)
        {
            return false;
        }
    }
}