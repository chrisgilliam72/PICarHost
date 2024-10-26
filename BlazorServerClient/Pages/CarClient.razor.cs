using Microsoft.AspNetCore.Components;
using PanTiltHatLib;

namespace BlazorServerClient.Pages;

partial class CarClient
{
    [Inject]
    public ILogger<CarClient>? Logger { get; set; }
    [Inject]
    public IPanTiltService? PanTiltService {get;set;}

    protected override async Task OnInitializedAsync()
    {
        if (PanTiltService!=null)
            PanTiltService.Init(0x40,50);

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
