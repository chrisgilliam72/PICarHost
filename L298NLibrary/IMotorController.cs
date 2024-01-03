using Microsoft.Extensions.Logging;

namespace L298NLibrary
{
    public interface IMotorController
    {
        double SpeedFactor { get; }
        void Init(int IN1, int IN2, int IN3, int IN4);
        void CleanUp();
        double UpdateSpeedFactor(double speedFactor);
        void StartForward();
        void StartBackwards();
        void StartTurnLeft();
        void StartTurnRight();
        void Stop();
    }
}




