using DataStreamHub;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddSignalR();
var app = builder.Build();
app.MapHub<DataStreamHub.DataStreamHub>("/DataStream");
// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.Run("http://localhost:8070");


