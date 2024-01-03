using Iot.Device.Media;
//testing
public class Camera
{
    public bool IsCapturing{ get  => device.IsCapturing; } 
    VideoConnectionSettings settings;
    VideoDevice device;
    CancellationTokenSource tokenSource = new CancellationTokenSource();

    public event VideoDevice.NewImageBufferReadyEvent NewImageReady
    {
        add { device.NewImageBufferReady += value; }
        remove { device.NewImageBufferReady -= value; }
    }

    public Camera()
    {
        settings = new VideoConnectionSettings(
            busId: 0,
            captureSize: (800, 600),
            pixelFormat: PixelFormat.JPEG
        );
        settings.Brightness=50;
        settings.ExposureType=ExposureType.Auto;
        settings.WhiteBalanceEffect=WhiteBalanceEffect.Auto;
        settings.VerticalFlip=true;
        device = VideoDevice.Create(settings);
        device.ImageBufferPoolingEnabled = true;
    }

    public void StartCapture()
    {
        if (!device.IsOpen)
        {
            device.StartCaptureContinuous();
        }

        if (!device.IsCapturing)
        {
            new Thread(() =>
                {
                    device.CaptureContinuous(tokenSource.Token);
                }
            ).Start();
        }
    }

    public void StopCapture()
    {
        if (device.IsCapturing)
        {
            tokenSource.Cancel();
            tokenSource = new CancellationTokenSource();
            device.StopCaptureContinuous();
        }
    }
}
