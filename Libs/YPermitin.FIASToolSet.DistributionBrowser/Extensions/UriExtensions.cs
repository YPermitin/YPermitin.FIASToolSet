using System;

namespace YPermitin.FIASToolSet.DistributionBrowser.Extensions
{
    public static class UriExtensions
    {
        public static string GetAbsoluteUri(this Uri sourceUri)
        {
            if (sourceUri == null)
                return string.Empty;

            if(sourceUri.IsAbsoluteUri)
                return sourceUri.AbsoluteUri;

            return string.Empty;
        }
    }
}
