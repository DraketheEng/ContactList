namespace ContactManagement.RabbitMqService;

public interface IRabbitMqService
{
    void PublishMessage(string message);
}