
using Microsoft.AspNetCore.Components;
namespace BlazorServerClient.Pages;

partial class CarClient
{
    [Inject]
    public ILogger<CarClient> Logger { get; set; }


    protected override async Task OnInitializedAsync()
    {
        // Define the hub connection

    }
    void OnCameraUp()
    {
        Logger.LogInformation("Camera Up");


    }

    async Task OnCameraDown()
    {
        Logger.LogInformation("Camera Down");


    }

    void OnCameraRight()
    {
        Logger.LogInformation("Camera Right");

    }

    void OnCameraLeft()
    {
        Logger.LogInformation("Camera Left");


    }
    void OnCarRight()
    {

        Logger.LogInformation("Car Right");
    }


    void OnCarLeft()
    {

        Logger.LogInformation("Car Left");
    }

    void OnCarForward()
    {

        Logger.LogInformation("Car Forward");
    }
    void OnCarBack()
    {

        Logger.LogInformation("Car Back");
    }

    void OnCarSlower()
    {

        Logger.LogInformation("Car Slower");
    }

    void OnCarFaster()
    {

        Logger.LogInformation("Car Faster");
    }


    void OnCarStop()
    {
        Logger.LogInformation("Car Stop");
    }
}
