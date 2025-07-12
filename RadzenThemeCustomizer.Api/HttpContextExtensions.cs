namespace RadzenThemeCustomizer.Api;

public static class HttpContextExtensions
{
    public static string GetUserId(this HttpContext context)
    {
        //var ipAddress = context.Connection.RemoteIpAddress?.ToString();

        //if (!string.IsNullOrWhiteSpace(ipAddress))
        //{
        //    return ipAddress;
        //}

        return "123";

        //throw new InvalidOperationException("Unable to determine client IP address.");
    }
}
