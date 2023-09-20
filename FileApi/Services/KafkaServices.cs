using Confluent.Kafka;
using FileApi.Configs;

namespace FileApi.Services;

public class KafkaServices
{
    private readonly KafkaConfigs _kafkaConfigs;
    private readonly ProducerConfig _config = new ();
    
    public KafkaServices(KafkaConfigs kafkaConfigs)
    {
        _kafkaConfigs = kafkaConfigs;
        _config.BootstrapServers = kafkaConfigs.Host;
    }

    public async Task SendToKafka(string message)
    {
        using (var producer = new ProducerBuilder<Null, string>(_config).Build())
        {
            try
            {
                await producer.ProduceAsync(_kafkaConfigs.Topic, new Message<Null, string> { Value = message });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка: {e}");
            }
        }
    }
}