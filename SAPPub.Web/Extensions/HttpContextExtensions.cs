namespace SAPPub.Web.Extensions;

public static class HttpContextExtensions
{
    public static void Set<T>(this HttpContext context, string key, T value)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("The key cannot be null or empty", nameof(key));
        }

        context.Items[key] = value;
    }

    public static T? Get<T>(this HttpContext context, string key)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("The key cannot be null or empty", nameof(key));
        }

        if (context.Items.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }

        return default;
    }
}
