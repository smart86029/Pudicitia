namespace Pudicitia.Common.RabbitMQ;

public class RabbitMQOptions
{
    public string ConnectionString { get; set; } = string.Empty;

    public string QueueName { get; set; } = string.Empty;
}
