using FileConsumer.Configs;
using FileConsumer.Database;
using FileConsumer.Handlers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BrokerContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
var kafkaConfigs = new KafkaConfigs();
builder.Configuration.GetSection(nameof(KafkaConfigs)).Bind(kafkaConfigs);
builder.Services.AddSingleton(kafkaConfigs);
builder.Services.AddHostedService<KafkaConsumerHandler>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();