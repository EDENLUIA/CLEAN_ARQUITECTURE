
using Confluent.Kafka;
using System.Text.Json;
using Audit.Core.Common;
using Audit.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.Extensions.Logging;

namespace Audit.Infrastructure.Repositories
{
    public class ProducerRepository : IProducerRepository
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<ProducerRepository> _logger;
        public ProducerRepository(IConfiguration configuration, ILogger<ProducerRepository> logger)
        {           
           _configuration = configuration;
           _logger = logger;
        }
        public async Task<bool> SendAsync(string topic, OperationEvent operationEvent)
        {

            try
            {

                var config = new ProducerConfig
                {
                    //BootstrapServers = $"{_kafkaSettings.Hostname}:{_kafkaSettings.Port}"
                    BootstrapServers = _configuration["kafka:Host"],
                    SocketTimeoutMs = Convert.ToInt32(_configuration["kafka:SocketTimeoutMs"]),
                    MessageTimeoutMs = Convert.ToInt32(_configuration["kafka:MessageTimeoutMs"]),

                };

                using var producer = new ProducerBuilder<string, string>(config)
                .SetKeySerializer(Serializers.Utf8)
                .SetValueSerializer(Serializers.Utf8)
                .Build();

                var eventMessage = new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = JsonSerializer.Serialize(operationEvent)
                };

                var deliveryStatus = await producer.ProduceAsync(topic, eventMessage);

                if (deliveryStatus.Status == PersistenceStatus.NotPersisted)
                {
                    _logger.LogError(@$" No se pudo enviar el mensaje {operationEvent.GetType().Name} 
               hacia el topic - {topic}, 
               por la siguiente razon: {deliveryStatus.Message}");

                    return false;
                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }

           
        }
    }
}
