namespace RadzenThemeCustomizer.Api;

public static class HttpContextExtensions
{
    public static string GetUserId(this HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress;

        if (ip != null)
        {
            if (ip.IsIPv4MappedToIPv6)
            {
                ip = ip.MapToIPv4();
            }

            return ip.ToString();
        }

        throw new InvalidOperationException("Unable to determine client IP address.");
    }
}
