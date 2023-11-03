
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Monitoring.Abstractions;

internal class TimedOperation : ITimedOperation
{
    private readonly ILogger _logger;
    private readonly string _operationName;
    private OperationResult _result;
    private readonly Stopwatch _stopwatch;

    public TimedOperation(ILogger logger, string operationName)
    {
        _logger = logger;
        _operationName = operationName;
        _result = OperationResult.UnMarked;
        _stopwatch = new Stopwatch();
        Start();
    }

    private void Start()
    {
        _logger.LogInformation("{Operation} Started", _operationName);
        _stopwatch.Start();
    }

    public void MarkComplete() => _result = OperationResult.Completed;
    public void MarkFail() => _result = OperationResult.Failed;

    public void Dispose()
    {
        var outcome = _result switch
        {
            OperationResult.Failed => "Failed",
            OperationResult.Completed => "Completed",
            _ => "Finished"
        };

        _stopwatch.Stop();
        _logger.LogInformation("{Operation} finished in {Elapsed} ms. Outcome {Outcome}",
            _operationName,
            _stopwatch.ElapsedMilliseconds,
            outcome);
    }


    enum OperationResult
    {
        UnMarked,
        Completed,
        Failed
    }
}
