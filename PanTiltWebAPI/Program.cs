using PanTiltHatLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IPanTiltService,PanTiltService>();   
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var panTiltService= app.Services.GetService<IPanTiltService>();
if (panTiltService!=null)
    panTiltService.Init(0x40,50);

app.MapGet("/Up", ([FromServices] IPanTiltService panTiltService) =>
{
    panTiltService.Up();
})
.WithOpenApi();


app.MapGet("/Down", ([FromServices] IPanTiltService panTiltService) =>
{
    panTiltService.Down();
})
.WithOpenApi();

app.MapGet("/Left", ([FromServices] IPanTiltService panTiltService) =>
{
    panTiltService.Left();
})
.WithOpenApi();

app.MapGet("/Right", ([FromServices] IPanTiltService panTiltService) =>
{
    panTiltService.Right();
})
.WithOpenApi();

app.MapGet("/Reset", ([FromServices] IPanTiltService panTiltService) =>
{
    panTiltService.Reset();
})
.WithOpenApi();

app.Run("http://localhost:8085");


