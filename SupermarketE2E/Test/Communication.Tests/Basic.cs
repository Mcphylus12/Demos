using System.Threading;
using System.Threading.Tasks;
using Communication.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Communication.Tests;
public class Basic
{
    [Fact]
    public async Task BasicRun()
    {
        var services = new ServiceCollection();
        services.AddCommunication(typeof(Basic));
        var prov = services.BuildServiceProvider();

        var handler = new HandlerManager(prov);

        var result = (TestResponse)await handler.HandlerWithResponse(typeof(TestRequest), new TestRequest() { In = 4 });

        Assert.True(result.Out == 9);
    }
}

public class TestRequest : IRequest<TestResponse>
{
    public string RequestType => "TestMessage";

    public int In { get; set; }
}

public class TestResponse
{
    public int Out { get; set; }
}

public class TestHandler : IRequestHandler<TestRequest, TestResponse>
{
    public Task<TestResponse> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new TestResponse { Out = request.In + 5 });
    }
}
