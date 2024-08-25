
using CameraLibrary;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ICamera, LibCamera>();
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

var app = builder.Build();



app.UseHttpsRedirection();

byte[] CreateHeader(int length)
{
    string header =
        $"--frame\r\nContent-Type:image/jpeg\r\nContent-Length:{length}\r\n\r\n";
    return System.Text.Encoding.ASCII.GetBytes(header);
}

byte[] CreateFooter()
{
    return System.Text.Encoding.ASCII.GetBytes("\r\n");
}

app.MapGet("/video", async (HttpContext context, [FromServices] ICamera libCamera, [FromServices] ILoggerFactory loggerFactory) =>
{
    var logger = loggerFactory.CreateLogger("API Endpoint rebuilt");
    logger.LogInformation("Image Endpoint with context hit  ");
    var bufferingFeature = context.Response.HttpContext.Features.Get<IHttpResponseBodyFeature>();
    bufferingFeature?.DisableBuffering();

    context.Response.StatusCode = 200;
    context.Response.ContentType = "multipart/x-mixed-replace; boundary=--frame";
    context.Response.Headers.Append("Connection", "Keep-Alive");
    context.Response.Headers.Append("CacheControl", "no-cache");

    libCamera.StartCapture();

    while (!context.RequestAborted.IsCancellationRequested)
    {
        // logger.LogInformation("Streaming Image... ");
        if (libCamera.HasImages())
        {
            var imageBytes = libCamera.GetImage();
            // logger.LogDebug($"Image removed: Buffer length:{Images.Count}");
            await context.Response.BodyWriter.WriteAsync(CreateHeader(imageBytes.Length));
            await context.Response.BodyWriter.WriteAsync(imageBytes);
            await context.Response.BodyWriter.WriteAsync(CreateFooter());
        }

    }

    logger.LogInformation("Endpoint Exiting.");
    libCamera.StopCapture();
});

app.Run("http://localhost:8080");


