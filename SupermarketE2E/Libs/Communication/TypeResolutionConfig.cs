namespace Communication;

public class TypeResolutionConfig
{
    public Dictionary<string, Type> RequestMap { get; internal set; } = new Dictionary<string, Type>();
    public Dictionary<string, Type> MessageMap { get; internal set; } = new Dictionary<string, Type>();
}
