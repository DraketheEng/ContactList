using System;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ContactManagement.RabbitMqService
{
    public class RabbitMqService : IHostedService
    {
        private readonly ILogger<RabbitMqService> _logger;
        private readonly IOptions<RabbitMqConfiguration> _rabbitMqConfig;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqService(ILogger<RabbitMqService> logger, IOptions<RabbitMqConfiguration> rabbitMqConfig)
        {
            _logger = logger;
            _rabbitMqConfig = rabbitMqConfig;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("RabbitMQ Service is starting.");

            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqConfig.Value.HostName,
                Port = _rabbitMqConfig.Value.Port,
                UserName = _rabbitMqConfig.Value.UserName,
                Password = _rabbitMqConfig.Value.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "report_request_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                HandleReportRequest(message);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: "report_request_queue", autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("RabbitMQ Service is stopping.");

            _channel.Close(200, "Goodbye");
            _connection.Close();

            return Task.CompletedTask;
        }

        private void HandleReportRequest(string message)
        {
            var reportRequest = JsonConvert.DeserializeObject<ReportRequestMessage>(message);
            _logger.LogInformation($"Received report request message: {reportRequest.ReportId}");
        }
    }

    public class ReportRequestMessage
    {
        public string ReportId { get; set; }
    }
}
