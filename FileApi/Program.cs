
using FileApi.Configs;
using FileApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var kafkaConfigs = new KafkaConfigs();
builder.Configuration.GetSection(nameof(KafkaConfigs)).Bind(kafkaConfigs);
builder.Services.AddSingleton(kafkaConfigs);
builder.Services.AddScoped<KafkaServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();