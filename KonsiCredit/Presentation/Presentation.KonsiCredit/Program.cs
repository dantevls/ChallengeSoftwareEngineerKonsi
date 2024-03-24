using Infra.CrossCutting.KonsiCredit;
using Infra.CrossCutting.KonsiCredit.CachingRegistrable;
using Services.KonsiCredit.QueueAppService;
using Registrable = Infra.CrossCutting.KonsiCredit.CachingRegistrable.Registrable;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAuthentication("MyAuthenticationScheme")
    .AddCookie("MyAuthenticationScheme", options =>
    {
        options.Cookie.Name = "MyCookieName";
    });

builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Infra.CrossCutting.KonsiCredit.Registrable.RegisterServices(builder.Services);
Registrable.RegisterServices(builder.Services);

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
