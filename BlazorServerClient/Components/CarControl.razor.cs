using Microsoft.AspNetCore.Components;
using L298NLibrary;

namespace BlazorServerClient.Components;

public partial class CarControl
{
    [Inject]
    IMotorController? MotorController { get; set; }
    [Inject]
    public ILogger<CarControl>? Logger { get; set; }
    private double SpeedFactor {get;set;}=0.5;
    protected override void OnInitialized()
    {
        if (MotorController!=null)    
        {
            MotorController.Init(7,1,21,20);
            SpeedFactor=MotorController.UpdateSpeedFactor(0.5);
        }   
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
            SpeedFactor = MotorController.UpdateSpeedFactor(SpeedFactor - 0.1);
            Logger?.LogInformation("Car Slower");
        }

    }

    void OnCarFaster()
    {
        if (MotorController is not null)
        {
            SpeedFactor = MotorController.UpdateSpeedFactor(SpeedFactor + 0.1);
            Logger?.LogInformation("Car Faster");
        }
    }


    void OnCarStop()
    {
        MotorController?.Stop();
        Logger?.LogInformation("Car Stop");
    }
}
