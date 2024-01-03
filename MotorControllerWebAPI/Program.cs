using Microsoft.AspNetCore.Mvc;
using L298NLibrary;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMotorController,L298NMotorProcessor>();
var app = builder.Build();
var motorController=app.Services.GetService<IMotorController>();

motorController?.Init(21,20,1,7);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/back", ([FromServices] IMotorController  motorCntrller ) =>
{
    motorCntrller.StartBackwards();
})
.WithName("back")
.WithOpenApi();

app.MapGet("/forward", ([FromServices] IMotorController  motorCntrller) =>
{
    motorCntrller.StartForward();
})
.WithName("forward")
.WithOpenApi();

app.MapGet("/left", ([FromServices] IMotorController  motorCntrller) =>
{
    motorCntrller.StartTurnLeft();
})
.WithName("left")
.WithOpenApi();

app.MapGet("/right", ([FromServices] IMotorController  motorCntrller) =>
{
    motorCntrller.StartTurnRight();
})
.WithName("right")
.WithOpenApi();

app.MapGet("/stop", ([FromServices] IMotorController  motorCntrller) =>
{
    motorCntrller.Stop();
})
.WithName("stop")
.WithOpenApi();

app.MapGet("/faster", ([FromServices] IMotorController  motorCntrller) =>
{
    var speed= motorCntrller.SpeedFactor;
    motorCntrller.UpdateSpeedFactor(speed+0.1);
})
.WithName("faster")
.WithOpenApi();

app.MapGet("/slower", ([FromServices] IMotorController  motorCntrller) =>
{
    var speed= motorCntrller.SpeedFactor;
    motorCntrller.UpdateSpeedFactor(speed-0.1);
})
.WithName("slower")
.WithOpenApi();

app.Run("http://localhost:8095");

