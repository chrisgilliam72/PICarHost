using Microsoft.AspNetCore.Components;
using PanTiltHatLib;
using Ultraborg;
using CameraLibrary;
namespace BlazorServerClient.Pages;

partial class CarClient
{
    [Inject]
    public ILogger<CarClient>? Logger { get; set; }
    [Inject]
    public IPanTiltService? PanTiltService {get;set;}
    [Inject]
    public IUltraborgAPI? UltraborgAPI {get;set;}
    [Inject]
    public ICamera? Camera {get;set;}
    private Timer _distanceTimer;
    private Timer __imageTimer;
    private bool _isImagerRunning = false;
    private bool _isDistanceSensorRunning = false;
    private string Distance {get;set;}
    private double LastDistance {get;set;}
     private byte[] imageData;
    protected override async Task OnInitializedAsync()
    {
        if (PanTiltService!=null)
            PanTiltService.Init(0x40,50);
        if (UltraborgAPI!=null)         
            UltraborgAPI.Setup();   
        if (Camera!=null)
            Camera.StartCapture();
        LastDistance=0.0;
        PollDistance();
        PollImages();
    }

    private void PollImages()
    {
       if (!_isImagerRunning)
        {
            _isImagerRunning = true;
            __imageTimer = new Timer(async _ =>
            {
                await InvokeAsync( async () =>
                {
                    if (Camera.HasImages())
                    {
                        var tmpData=Camera.GetImage();
                        if (tmpData!=null)
                        {
                            Logger.LogInformation($"Image Data: {tmpData.Length}");
                            imageData = tmpData;
                            StateHasChanged(); // Update the UI if needed
                        }

                    }
                    
                });

            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100)); // Call every 10 seconds
        }
    }
    private void PollDistance()
    {
       if (!_isDistanceSensorRunning)
        {
            _isDistanceSensorRunning = true;
            _distanceTimer = new Timer(async _ =>
            {
                await InvokeAsync( async () =>
                {
                    var ubDistance =UltraborgAPI?.GetDistance(1);
                    if (ubDistance.HasValue && Math.Abs(LastDistance-ubDistance.Value)>0.5)
                    {
                        Distance=string.Format("{0:F1}",ubDistance);
                    }

                    
                    StateHasChanged(); // Update the UI if needed
                });

            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100)); // Call every 10 seconds
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
    void OnCarRight()
    {
        Logger?.LogInformation("Car Right");
    }


    void OnCarLeft()
    {
        Logger?.LogInformation("Car Left");
    }

    void OnCarForward()
    {
        Logger?.LogInformation("Car Forward");
    }
    void OnCarBack()
    {
        Logger?.LogInformation("Car Back");
    }

    void OnCarSlower()
    {
        Logger?.LogInformation("Car Slower");
    }

    void OnCarFaster()
    {
        Logger?.LogInformation("Car Faster");
    }


    void OnCarStop()
    {
        Logger?.LogInformation("Car Stop");
    }
}
