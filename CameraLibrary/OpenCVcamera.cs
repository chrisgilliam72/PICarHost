using OpenCvSharp;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CameraLibrary;

public class OpenCVCamera : ICamera, IDisposable
{
    private readonly ILogger<OpenCVCamera> _logger;
    private VideoCapture _capture;
    private Mat? _frame=null!;
    private bool _isCapturing=false;
    private Task? _captureTask=null!;
    private CancellationTokenSource? _cancellationTokenSource=null!;
    private readonly object _lock = new();
    private byte[] _latestImage=null!;

    public OpenCVCamera(ILogger<OpenCVCamera> logger)
    {
        _logger = logger;
        _frame = new Mat();
        _capture = new VideoCapture("libcamerasrc awb-mode=fluorescent ! video/x-raw,width=1280,height=720 ! videoconvert !  appsink", VideoCaptureAPIs.GSTREAMER);

    }

    public bool IsCapturing => _isCapturing;

    public void StartCapture()
    {
        if (_isCapturing)
            return;

        _logger.LogInformation("Starting OpenCV capture...");
        _cancellationTokenSource = new CancellationTokenSource();


        if (!_capture.IsOpened())
        {
            _logger.LogError("Failed to open camera");
            return;
        }

        _isCapturing = true;
        _captureTask = Task.Run(() => CaptureLoop(_cancellationTokenSource.Token));
    }

    public void StopCapture()
    {
        if (!_isCapturing)
            return;

        _logger.LogInformation("Stopping OpenCV capture...");
        _cancellationTokenSource?.Cancel();
        _captureTask?.Wait();
        _capture.Release();
        _isCapturing = false;
    }

    private async Task CaptureLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (_frame!=null && _capture.Read(_frame) && !_frame.Empty())
            {
                Cv2.Flip(_frame, _frame, FlipMode.X); // Flip vertically
                Cv2.ImEncode(".jpg", _frame, out var buffer);
                lock (_lock)
                {
                    _latestImage = buffer.ToArray();
                }
            }

            await Task.Delay(33,token);
        }
    }

    public bool HasImages()
    {
        lock (_lock)
        {
            return _latestImage != null;
        }
    }

    public byte[] GetImage()
    {
        lock (_lock)
        {
            if (_latestImage!=null)
                return _latestImage;
            return  Array.Empty<byte>();               
        }
    }

    public void Dispose()
    {
        _capture?.Dispose();
        _frame?.Dispose();
        _cancellationTokenSource?.Dispose();
    }
}
