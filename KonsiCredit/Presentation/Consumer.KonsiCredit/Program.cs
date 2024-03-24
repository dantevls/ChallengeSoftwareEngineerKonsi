using Consumer.KonsiCredit.Consumer;
using Infra.CrossCutting.KonsiCredit;
using Infra.CrossCutting.KonsiCredit.CachingRegistrable;
using Registrable = Infra.CrossCutting.KonsiCredit.CachingRegistrable.Registrable;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<RabbitConsumer>();
Registrable.RegisterServices(builder.Services);
Infra.CrossCutting.KonsiCredit.Registrable.RegisterServices(builder.Services);

var app = builder.Build();

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
