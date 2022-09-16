using System.Runtime.InteropServices;

namespace YPermitin.FIASToolSet.API.Infrastructure
{
    /// <summary>
    /// Вспомогательные методы определения операционной системы
    /// </summary>
    public static class OperatingSystemHelper
    {
        public static bool IsWindows() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static bool IsMacOS() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public static bool IsLinux() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    }
}
