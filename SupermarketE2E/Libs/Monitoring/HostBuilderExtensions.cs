using Microsoft.Extensions.Hosting;
using Serilog;

namespace Monitoring;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddLogging(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((hostBuilderContext, loggerConfiguration) =>
        {

        });
    }
}
