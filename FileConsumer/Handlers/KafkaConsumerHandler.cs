using Confluent.Kafka;
using FileConsumer.Configs;
using FileConsumer.Database;

namespace FileConsumer.Handlers;

public class KafkaConsumerHandler : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly KafkaConfigs _kafkaConfigs;

    public KafkaConsumerHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _kafkaConfigs = serviceProvider.GetRequiredService<KafkaConfigs>();
    }
    
    /// <summary>
    /// Ждем сообщений от продюсера
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var conf = new ConsumerConfig
        {
            GroupId = _kafkaConfigs.GroupId,
            BootstrapServers = _kafkaConfigs.Host,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        using (var builder = new ConsumerBuilder<Ignore, string>(conf).Build())
        {
            builder.Subscribe(_kafkaConfigs.Topic);
            var cancelToken = new CancellationTokenSource();
            try
            {
                while (true)
                {
                    var consumer = builder.Consume(cancelToken.Token);
                    Console.WriteLine($"Сообщение: {consumer.Message.Value} пришло от {consumer.TopicPartitionOffset}");
                    
                    // Записываем сообщение от продюсера в бд
                    SaveInDb(consumer.Message.Value);
                }
            }
            catch (Exception)
            {
                builder.Close();
            }
        }
        return Task.CompletedTask;
    }

    private void SaveInDb(string message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            try
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BrokerContext>();
                dbContext.Messages.Add(new Message() { Data = message });
                dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}