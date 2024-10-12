using DataStreamHub;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", builder =>
    {
        builder.WithOrigins("https://localhost:7180")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();  // Required for WebSockets
    });
});
var app = builder.Build();
app.UseCors("AllowBlazorClient");
app.MapHub<DataStreamHub.DataStreamHub>("/DataStream");
// Configure the HTTP request pipeline.


//app.UseHttpsRedirection();

app.Run("https://localhost:8070");


