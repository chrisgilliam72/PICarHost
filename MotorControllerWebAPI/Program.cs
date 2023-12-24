
using L298NLibrary;
var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

 var loggerFactory = builder.GetRequiredService<ILoggerFactory>();
var motorCntrller = new L298NMotorProcessor(21,20,1,7);
motorCntrller.InitControllers(); 

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/back", () =>
{
    motorCntrller.StartBackwards();
})
.WithName("back")
.WithOpenApi();

app.MapGet("/forward", () =>
{
    motorCntrller.StartForward();
})
.WithName("forward")
.WithOpenApi();

app.MapGet("/left", () =>
{
    motorCntrller.StartTurnLeft();
})
.WithName("left")
.WithOpenApi();

app.MapGet("/right", () =>
{
    motorCntrller.StartTurnRight();
})
.WithName("right")
.WithOpenApi();

app.MapGet("/stop", () =>
{
    motorCntrller.Stop();
})
.WithName("stop")
.WithOpenApi();

app.MapGet("/faster", () =>
{
    var speed= motorCntrller.SpeedFactor;
    motorCntrller.UpdateSpeedFactor(speed+0.1);
})
.WithName("faster")
.WithOpenApi();

app.MapGet("/slower", () =>
{
    var speed= motorCntrller.SpeedFactor;
    motorCntrller.UpdateSpeedFactor(speed-0.1);
})
.WithName("slower")
.WithOpenApi();

app.Run();


