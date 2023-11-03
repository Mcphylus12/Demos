using Microsoft.Extensions.Logging;

namespace Monitoring.Abstractions;

public static class TimedOperationExtensions
{
    public static ITimedOperation StartOperation(this ILogger logger, string operationName) => new TimedOperation(logger, operationName);
}
