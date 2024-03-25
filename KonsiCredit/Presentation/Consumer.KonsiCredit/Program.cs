using Consumer.KonsiCredit.Consumer;
using Infra.CrossCutting.ConsumerKonsiCredit;
using Services.KonsiCredit.QueueAppService;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<UserCpfConsumer>();

Registrable.RegisterServices(builder.Services);
Infra.CrossCutting.ConsumerKonsiCredit.CachingRegistrable.Registrable.RegisterServices(builder.Services, builder.Configuration);
Infra.CrossCutting.ConsumerKonsiCredit.ElasticSearchRegistrable.Registrable.RegisterServices(builder.Services, builder.Configuration);

var app = builder.Build();

var queueService = app.Services.GetService<IProducerQueueAppService>();
queueService?.EnqueueCpf();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
