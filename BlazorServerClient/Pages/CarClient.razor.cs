using Microsoft.AspNetCore.Components;
using PanTiltHatLib;
using Ultraborg;
using CameraLibrary;
using L298NLibrary;
namespace BlazorServerClient.Pages;

partial class CarClient
{
    //test commit new commit 
    [Inject]
    public ILogger<CarClient>? Logger { get; set; }
    [Inject]
    public IPanTiltService? PanTiltService {get;set;}
    [Inject]
    public IUltraborgAPI? UltraborgAPI {get;set;}
    [Inject]
    public ICamera? Camera {get;set;}
    [Inject]
    IMotorController? MotorController  {get;set;}
    private Timer _distanceTimer  =null !;
    private Timer __imageTimer =null!;
    private bool _isImagerRunning = false;
    private bool _isDistanceSensorRunning = false;
    private string Distance {get;set;} = "";
    private double LastDistance {get;set;}
     private byte[] imageData = Array.Empty<byte>();
    private double SpeedFactor {get;set;}=0.5;
    protected override void OnInitialized()
    {
         if (PanTiltService!=null)
                PanTiltService.Init(0x40,50);
            // if (UltraborgAPI!=null)         
            //     UltraborgAPI.Setup();   
            if (Camera!=null)
                Camera.StartCapture();
            // if (MotorController!=null)    
            // {
            //     MotorController.Init(7,1,21,20);
            //     SpeedFactor=MotorController.UpdateSpeedFactor(0.5);
            // }      

            LastDistance=0.0;
    }
    protected override async void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {           
            // PollDistance();
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
                await InvokeAsync(  () =>
                {
                    if (Camera is not null && Camera.HasImages())
                    {
                        var tmpData=Camera.GetImage();
                        if (tmpData!=null)
                        {
                            Logger?.LogDebug($"Image Data: {tmpData.Length}");
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
                await InvokeAsync(  () =>
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
        MotorController?.StartTurnRight();
        Logger?.LogInformation("Car Right");
    }


    void OnCarLeft()
    {
        MotorController?.StartTurnLeft();
        Logger?.LogInformation("Car Left");
    }

    void OnCarForward()
    {
        MotorController?.StartForward();
        Logger?.LogInformation("Car Forward");
    }
    void OnCarBack()
    {
        MotorController?.StartBackwards();
        Logger?.LogInformation("Car Back");
    }

    void OnCarSlower()
    {
        if (MotorController is not null)
        {
            SpeedFactor=MotorController.UpdateSpeedFactor(SpeedFactor-0.1);
            Logger?.LogInformation("Car Slower");
        }

    }

    void OnCarFaster()
    {
        if (MotorController is not null)
        {
            SpeedFactor=MotorController.UpdateSpeedFactor(SpeedFactor+0.1);
            Logger?.LogInformation("Car Faster");
        }        
    }


    void OnCarStop()
    {
        MotorController?.Stop();
        Logger?.LogInformation("Car Stop");
    }
}
