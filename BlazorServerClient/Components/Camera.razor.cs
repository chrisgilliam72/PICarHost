using BlazorServerClient.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using PanTiltHatLib;

namespace BlazorServerClient.Components;

public partial class Camera : IDisposable
{
    //test commit new commit 
    [Inject]
    public ILogger<Camera>? Logger { get; set; }
    [Inject]
    public IPanTiltService? PanTiltService { get; set; }

    [Inject]
    public ICameraService? CameraService { get; set; }

    private Timer __imageTimer = null!;
    private bool _isImagerRunning = false;

    private byte[] imageData = Array.Empty<byte>();

    protected override void OnInitialized()
    {
        if (PanTiltService != null)
            PanTiltService.Init(0x40, 50);

        if (CameraService != null)
        {
            CameraService.Start();
            Logger?.LogDebug("Camera started.");
        }




    }
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {

            PollImages();
        }

    }

    private void PollImages()
    {
        if (!_isImagerRunning)
        {
            _isImagerRunning = true;
            __imageTimer = new Timer(async _ =>
            {
                await InvokeAsync(() =>
                {
                    var camera = CameraService?.Camera;
                    if (camera is not null && camera.HasImages())
                    {
                        if (!camera.IsCapturing)
                        {
                            Logger?.LogWarning("Camera is not capturing");
                            __imageTimer?.Dispose();
                            _isImagerRunning = false;
                            return;
                        }
                        else
                        {
                            var tmpData = camera.GetImage();
                            if (tmpData != null && tmpData.Length > 0)
                            {


                                // Logger?.LogDebug($"Image Data: {tmpData.Length}");
                                if (camera.ImageSignificantlyChanged())
                                {
                                    Logger?.LogDebug("Image change detected.");

                                    imageData = tmpData;
                                    StateHasChanged(); // Only update UI if image changed
                                }
                            }
                        }
                    }

                });

            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(50)); // Call every 10 seconds
        }
    }


    void OnKeyDown(KeyboardEventArgs e)
    {


        switch (e.Key)
        {
            case "ArrowUp":
                OnCameraUp();
                break;
            case "ArrowDown":
                OnCameraDown();
                break;
            case "ArrowLeft":
                OnCameraLeft();
                break;
            case "ArrowRight":
                OnCameraRight();
                break;
        }
    }

    void OnCameraUp()
    {
        PanTiltService?.Up();
        Logger?.LogInformation("Camera Up");

    }

    void OnCameraDown()
    {
        PanTiltService?.Down();
        Logger?.LogInformation("Camera Down");
    }

    void OnCameraRight()
    {
        PanTiltService?.Right();
        Logger?.LogInformation("Camera Right");
    }

    void OnCameraLeft()
    {
        PanTiltService?.Left();
        Logger?.LogInformation("Camera Left");
    }


    public void Dispose()
    {
        __imageTimer?.Dispose();
        _isImagerRunning = false;

        Logger?.LogInformation("WebCam component disposed, capture stopped.");
    }
}
