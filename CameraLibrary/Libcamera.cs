
using Iot.Device.Common;
using Iot.Device.Media;
using Iot.Device.Camera.Settings;
using Microsoft.Extensions.Logging;
namespace CameraLibrary;


public class LibCamera : ICamera
{


    private ProcessRunner _processRunner;
    private string[] _cmdArgs;
    private readonly ILogger<LibCamera> _logger;
    private readonly List<byte[]> _Images = new List<byte[]>();
    public LibCamera(ILogger<LibCamera> logger)
    {
        _isCapturing = false;
        var builder = new CommandOptionsBuilder()
            .With(new CommandOptionAndValue(CommandOptionsBuilder.Get(Command.Nopreview)))
            .WithTimeout(1)
            // .WithVflip()
            // .WithHflip()
            .WithPictureOptions(90, "jpg")
            .WithResolution(1080, 720);
        _cmdArgs = builder.GetArguments();
        var processSettings = ProcessSettingsFactory.CreateForLibcamerastill();
        _processRunner = new ProcessRunner(processSettings);
        _logger = logger;
    }

    public byte[] GetImage()
    {
        var imageBytes = _Images.First();
        _Images.RemoveAt(0);
        _logger.LogDebug($"Image removed: Buffer length:{_Images.Count}");
        return imageBytes;
    }


    public bool HasImages()
    {
        return _Images.Any();
    }

    private void CaptureImages()
    {
        new Thread(async () =>
        {

            while (_isCapturing)
            {
                using (var memStream = new MemoryStream())
                {

                    await _processRunner.ExecuteAsync(_cmdArgs, memStream);
                    var imageBytes = memStream.ToArray();
                    _Images.Add(imageBytes);
                }

                _logger.LogDebug($"Image added: Buffer length:{_Images.Count}");
                Thread.Sleep(10);
            }

        }
        ).Start();
    }

    private bool _isCapturing;
    public bool IsCapturing => _isCapturing;

    public void StartCapture()
    {
        if (!_isCapturing)
        {
            _Images.Clear();
            _logger.LogInformation("Capturing started");
            CaptureImages();
            _isCapturing = true;
        }
    }

    public void StopCapture()
    {
        _isCapturing = false;
        _logger.LogInformation("Capturing stopped");
    }
}