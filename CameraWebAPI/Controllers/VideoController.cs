
using Microsoft.AspNetCore.Mvc;
using Iot.Device.Media;
using Microsoft.AspNetCore.Http.Features;
using System.Buffers;
using System.Text;

namespace dotnetPiCamServer.Controllers;

[ApiController]
[Route("[controller]")]
public class VideoController : ControllerBase
{
    private readonly ILogger<VideoController> _logger;
    private readonly Camera _camera;
    private readonly List<NewImageBufferReadyEventArgs> _eventList;
    public VideoController(ILogger<VideoController> logger, Camera camera)
    {
        _logger = logger;
        _camera = camera;
        _eventList= new List<NewImageBufferReadyEventArgs>();
    }

    [HttpGet(Name = "GetVideo")]
    public void Get()
    {
        Thread streamingThread;
        var bufferingFeature =
            HttpContext.Response.HttpContext.Features.Get<IHttpResponseBodyFeature>();
        bufferingFeature?.DisableBuffering();

        HttpContext.Response.StatusCode = 200;
        HttpContext.Response.ContentType = "multipart/x-mixed-replace; boundary=--frame";
        // HttpContext.Response.Headers.Add("Connection", "Keep-Alive");
        // HttpContext.Response.Headers.Add("CacheControl", "no-cache");
        _camera.NewImageReady += PushFrame;

        try
        {
            _logger.LogWarning($"Start streaming video");
            streamingThread=  new Thread(StreamFrames);
            _camera.StartCapture();
            Thread.Sleep(100);
            streamingThread.Start();

            while (!HttpContext.RequestAborted.IsCancellationRequested) { }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in streaming: {ex}");
        }
        finally
        {
            HttpContext.Response.Body.Close();
            _logger.LogInformation("Stop streaming video");
        }

        _camera.NewImageReady -= PushFrame;
        _camera.StopCapture();
        
    }

    private void PushFrame(object sender, NewImageBufferReadyEventArgs e)
    {
        
        _eventList.Add(e);
        _logger.LogInformation("Frame pushed. Event list length="+_eventList.Count());
    }
    private void PopFrame()
    {
        if (_eventList.Count>0)
        {
            var lastFrame=_eventList[0];
            if (lastFrame!=null)
            {
                _eventList.Remove(lastFrame);
                _logger.LogInformation("Frame popped. Event list length="+_eventList.Count());
                WriteFrame(lastFrame);
            }

        }       
    }

    private void StreamFrames()
    {
        _logger.LogInformation("StreamFrames");

        while (_camera.IsCapturing)
            PopFrame();

       _logger.LogInformation("StreamFrames Not capturing");
    }


    private async void WriteFrame(object sender,NewImageBufferReadyEventArgs e)
    {
        try
        {
            _logger.LogInformation("Writing frame with Length:"+e.Length);
            await HttpContext.Response.BodyWriter.WriteAsync(CreateHeader(e.Length));
            _logger.LogInformation("written header..");
            await HttpContext.Response.BodyWriter.WriteAsync(
                e.ImageBuffer.AsMemory().Slice(0, e.Length)
            );
            _logger.LogInformation("written frame..");
            await HttpContext.Response.BodyWriter.WriteAsync(CreateFooter());
            _logger.LogInformation("written footer..");
        }
        catch (ObjectDisposedException)
        {
            // ignore this as its thrown when the stream is stopped
        }

        ArrayPool<byte>.Shared.Return(e.ImageBuffer);
    }
    private async void WriteFrame(NewImageBufferReadyEventArgs e)
    {
        try
        {
            _logger.LogInformation("Writing frame with Length:"+e.Length);
            await HttpContext.Response.BodyWriter.WriteAsync(CreateHeader(e.Length));
            _logger.LogInformation("written header..");
            await HttpContext.Response.BodyWriter.WriteAsync(
                e.ImageBuffer.AsMemory().Slice(0, e.Length)
            );
            _logger.LogInformation("written frame..");
            await HttpContext.Response.BodyWriter.WriteAsync(CreateFooter());
            _logger.LogInformation("written footer..");
        }
        catch (ObjectDisposedException)
        {
            // ignore this as its thrown when the stream is stopped
        }

        ArrayPool<byte>.Shared.Return(e.ImageBuffer);
    }

    private byte[] CreateHeader(int length)
    {
        string header =
            $"--frame\r\nContent-Type:image/jpeg\r\nContent-Length:{length}\r\n\r\n";
        return Encoding.ASCII.GetBytes(header);
    }

    private byte[] CreateFooter()
    {
        return Encoding.ASCII.GetBytes("\r\n");
    }
}
