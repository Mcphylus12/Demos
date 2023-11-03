using System.Runtime.CompilerServices;

namespace Communication;

internal static class Guard
{
    public static T NotNull<T>(this T? o, [CallerArgumentExpression("o")] string? argName = null)
    {
        if (o is null) throw new ArgumentNullException(argName);
        return o;
    }
}
