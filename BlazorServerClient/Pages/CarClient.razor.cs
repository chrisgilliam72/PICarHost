using Microsoft.AspNetCore.Components;
using PanTiltHatLib;
using Ultraborg;
namespace BlazorServerClient.Pages;

partial class CarClient
{
    [Inject]
    public ILogger<CarClient>? Logger { get; set; }
    [Inject]
    public IPanTiltService? PanTiltService {get;set;}
    [Inject]
    public IUltraborgAPI? UltraborgAPI {get;set;}
    private Timer _timer;
    private bool _isRunning = false;
    private string Distance {get;set;}
    private double LastDistance {get;set;}
    protected override async Task OnInitializedAsync()
    {
        if (PanTiltService!=null)
            PanTiltService.Init(0x40,50);
        if (UltraborgAPI!=null)         
            UltraborgAPI.Setup();   
        LastDistance=0.0;
        PollDistance();
    }

    private void PollDistance()
    {
       if (!_isRunning)
        {
            _isRunning = true;
            _timer = new Timer(async _ =>
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
