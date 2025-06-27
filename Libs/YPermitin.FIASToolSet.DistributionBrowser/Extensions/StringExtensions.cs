using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("YPermitin.FIASToolSet.API.Tests")]
namespace YPermitin.FIASToolSet.DistributionBrowser.Extensions
{    
    public static class StringExtensions
    {
        public static Uri ToAbsoluteUri(this string sourceValue) 
        {
            if(string.IsNullOrEmpty(sourceValue))
            {
                return null;
            }

            if(Uri.TryCreate(sourceValue, UriKind.Absolute, out Uri parsedUri))
            {
                return parsedUri;
            }

            return null;
        }
    }
}
