namespace Pudicitia.Common.RabbitMQ;

public class RabbitMQOptions
{
    public Uri? Uri { get; set; }

    public string ClientName { get; set; } = string.Empty;

    public string ExchangeName { get; private init; } = "Pudicitia";
}
