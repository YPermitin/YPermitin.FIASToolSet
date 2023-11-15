using System.ComponentModel;
using System.Globalization;
using System.Xml;

namespace YPermitin.FIASToolSet.DistributionReader.Extensions;

internal static class XmlReaderExtensions
{
    public static bool GetAttributeAsBool(this XmlReader reader, string attributeName)
    {
        var sourceValue = reader.GetAttribute(attributeName);
        if (sourceValue == null)
        {
            return false;
        }
        else
        {
            if (int.TryParse(sourceValue, out int parsedInt))
            {
                return parsedInt > 0;
            }
            
            if (bool.TryParse(sourceValue, out bool parsedBool))
            {
                return parsedBool;
            }
        }

        return false;
    }
    
    public static DateOnly GetAttributeAsDateOnly(this XmlReader reader, string attributeName)
    {
        var sourceValue = reader.GetAttribute(attributeName);
        if (sourceValue == null)
        {
            return DateOnly.MinValue;
        }
        else
        {
            return GetAttributeWithOptions(reader, attributeName, DateOnly.MinValue,
                (sourceValueToParse) => DateOnly.Parse(
                    sourceValue, 
                    CultureInfo.InvariantCulture));
        }
    }
    
    public static int GetAttributeAsInt(this XmlReader reader, string attributeName)
    {
        return GetAttributeWithOptions(reader, attributeName, 0);
    }
    
    public static Guid GetAttributeAsGuid(this XmlReader reader, string attributeName)
    {
        return GetAttributeWithOptions(reader, attributeName, Guid.Empty);
    }
    
    public static string GetAttributeAsString(this XmlReader reader, string attributeName)
    {
        return GetAttributeWithOptions(reader, attributeName, string.Empty);
    }
    
    public static T GetAttributeWithOptions<T>(this XmlReader reader, string attributeName, 
        T defaultValue = default(T), Func<string, T> parseFunc = null)
    {
        var sourceValue = reader.GetAttribute(attributeName);
        if (sourceValue == null)
        {
            return defaultValue;
        }
        else
        {
            try
            {
                if (parseFunc != null)
                {
                    return parseFunc(sourceValue);
                }
                else
                {
                    if (typeof(T) == typeof(string))
                    {
                        return (T)Convert.ChangeType(sourceValue, typeof(T));
                    }
                    else
                    {
                        return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(sourceValue);
                    }
                }
            }
            catch
            {
                return defaultValue;
            }
            
        }
    }
}