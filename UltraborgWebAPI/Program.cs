using Microsoft.AspNetCore.Mvc;
using Ultraborg;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IUltraborgAPI,UltraborgAPI>();

var app = builder.Build();
var ultraBorgAPI=app.Services.GetService<IUltraborgAPI>();
ultraBorgAPI.Setup();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/Distance/{sensorNo}", ([FromServices]IUltraborgAPI ultraBorg, int sensorNo) =>
{

    // double distance = sensorNo switch
    // {
    //     1 => ultraBorg.GetFilteredDistance1(),
    //     2 => ultraBorg.GetFilteredDistance2(),
    //     3 => ultraBorg.GetFilteredDistance3(),
    //     4 => ultraBorg.GetFilteredDistance4(),
    //     _ => -1

    // };

    return ultraBorg.GetDistance(sensorNo);
}).WithName("GetDistance").WithOpenApi();
app.Run("http://localhost:8090");

