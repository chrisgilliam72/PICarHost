using CameraLibrary;

namespace BlazorServerClient.Services;

public class CameraSerice(ICamera camera) : ICameraService
{
    bool _cameraStarted = false;

    ICamera ICameraService.Camera => camera;

    public void Start()
    {
        if (!_cameraStarted)
        {
            camera.StartCapture();
            _cameraStarted = true;
        }

    }

    public void Stop()
    {
        _cameraStarted = false;
        camera.StopCapture();
    }


}