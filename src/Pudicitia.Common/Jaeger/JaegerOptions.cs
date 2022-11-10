namespace Pudicitia.Common.Jaeger;

public class JaegerOptions
{
    public string ServiceName { get; set; } = string.Empty;

    public string AgentHost { get; set; } = string.Empty;

    public int AgentPort { get; set; }
}
