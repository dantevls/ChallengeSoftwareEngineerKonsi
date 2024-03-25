using Infra.CrossCutting.KonsiCredit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAuthentication("MyAuthenticationScheme")
    .AddCookie("MyAuthenticationScheme", options =>
    { });

builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Registrable.RegisterServices(builder.Services);
Infra.CrossCutting.KonsiCredit.ElasticSearchRegistrable.Registrable.RegisterServices(builder.Services);
Infra.CrossCutting.KonsiCredit.CachingRegistrable.Registrable.RegisterServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
