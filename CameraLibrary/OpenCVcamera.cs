using OpenCvSharp;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CameraLibrary;

public class OpenCVCamera : ICamera, IDisposable
{
    private readonly ILogger<OpenCVCamera> _logger;
    private VideoCapture _capture;
    private Mat? _frame = null!;
    private Mat? _prevFrame =null!;
    private bool _isCapturing = false;
    private Task? _captureTask = null!;
    private CancellationTokenSource? _cancellationTokenSource = null!;
    private readonly object _lock = new();
    private byte[] _latestImage = null!;

    public OpenCVCamera(ILogger<OpenCVCamera> logger)
    {
        _logger = logger;
        _frame = new Mat();
        //_capture = new VideoCapture("libcamerasrc awb-mode=fluorescent ! video/x-raw,width=1280,height=720 ! videoconvert ! appsink", VideoCaptureAPIs.GSTREAMER);
        _capture = new VideoCapture("libcamerasrc ! video/x-raw,width=1280,height=720 ! videobalance brightness=0.0 ! videoconvert ! appsink", VideoCaptureAPIs.GSTREAMER);

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

        // Cancel the task gracefully
        _cancellationTokenSource?.Cancel();

        // Wait for the capture task to finish. Use a try-catch to handle potential issues during Wait().
        try
        {
            _captureTask?.Wait(); // Wait for the capture task to finish
        }
        catch (AggregateException ex)
        {
            // Handle specific exceptions if needed (e.g., TaskCanceledException)
            _logger.LogError($"Error stopping capture: {ex.Message}");
        }
        finally
        {
            // Release the capture device after the task completes
            _capture.Release();
            _isCapturing = false;

            // Dispose resources
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        _logger.LogInformation("OpenCV capture stopped.");
    }

    private async Task CaptureLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            using var newFrame = new Mat();
            if (_capture.Read(newFrame) && !newFrame.Empty())
            {
                Cv2.Flip(newFrame, newFrame, FlipMode.X); // Flip vertically

                lock(_lock)
                {
                    // Store previous frame as a deep clone
                    _prevFrame?.Dispose();
                    _prevFrame = _frame?.Clone();  // Clone the previous frame
                    _frame?.Dispose();
                    _frame = newFrame.Clone();     // Clone the new frame to keep as current
                }
                // Optionally encode and store image
                Cv2.ImEncode(".jpg", _frame, out var buffer);
                lock (_lock)
                {
                    _latestImage = buffer.ToArray();
                }
            }

            // If cancellation is requested, exit the loop gracefully
            try
            {
                await Task.Delay(50, token);
            }
            catch (TaskCanceledException)
            {
                // Ignore the exception if the task is canceled
                break;
            }
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
            if (_latestImage != null)
                return _latestImage;
            return Array.Empty<byte>();
        }
    }

    public void Dispose()
    {
        _capture?.Dispose();
        _frame?.Dispose();
        _cancellationTokenSource?.Dispose();
    }

    public bool ImageSignificantlyChanged(int pixelChangeThreshold)
    {
        lock(_lock)
        {
            if (_frame is null && _prevFrame is null)
                return false;

            if (_frame is null || _prevFrame is null)     
                return true;

            if (_frame.Size() != _prevFrame.Size())
                return true; // Resolution changed = definitely different

            using var diff = new Mat();
            Cv2.Absdiff(_frame, _prevFrame, diff);
            Cv2.CvtColor(diff, diff, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(diff, diff, 25, 255, ThresholdTypes.Binary); // 25 = small motion threshold
            int changedPixels = Cv2.CountNonZero(diff);
            if (changedPixels > pixelChangeThreshold)
                _logger.LogDebug($"Changed Pixel Value : {changedPixels}");
            return changedPixels > pixelChangeThreshold;
        }

    }
}
