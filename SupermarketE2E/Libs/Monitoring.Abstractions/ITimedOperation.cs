namespace Monitoring.Abstractions;

public interface ITimedOperation : IDisposable
{
    void MarkComplete();

    void MarkFail();
}
