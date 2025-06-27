using System;

namespace YPermitin.FIASToolSet.DistributionBrowser.Extensions
{
    internal static class StringExtensions
    {
        internal static Uri ToAbsoluteUri(this string sourceValue) 
        {
            if(Uri.TryCreate(sourceValue, UriKind.Absolute, out Uri parsedUri))
            {
                return parsedUri;
            }

            return null;
        }
    }
}
