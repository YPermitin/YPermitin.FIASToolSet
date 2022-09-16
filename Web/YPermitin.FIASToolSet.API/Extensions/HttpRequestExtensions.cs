using System.Net;

namespace YPermitin.FIASToolSet.API.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool IsLocal(this HttpContext context)
        {
            if (context.Connection.RemoteIpAddress == null)
                return false;

            if (context.Connection.RemoteIpAddress.Equals(context.Connection.LocalIpAddress))
            {
                return true;
            }

            return IPAddress.IsLoopback(context.Connection.RemoteIpAddress);
        }
    }
}
