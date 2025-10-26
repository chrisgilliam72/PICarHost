using CameraLibrary;

namespace BlazorServerClient.Services;

public interface ICameraService
{
    public void Start();
    public void Stop();
    ICamera Camera { get; }
}
