using Communication.Abstractions;

namespace DemoCore;
public class DemoMessage : IMessage
{
    public string MessageType => "DemoMessage";

    public string? Data { get; set; }
}

public class DemoRequest : IRequest
{
    public string RequestType => "DemoRequest";
    public string? Data { get; set; }

    public override string ToString()
    {
        return RequestType + Data;
    }
}
