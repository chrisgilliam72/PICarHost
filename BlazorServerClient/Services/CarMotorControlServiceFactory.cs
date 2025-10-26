using L298NLibrary;

internal class CarMotorControlServiceFactory : ICarMotorControlServiceFactory
{
    private readonly IMotorController _motorController;
    public CarMotorControlServiceFactory(IMotorController motorController)
    {
        _motorController=motorController;
    }

    public IMotorController CreateMotorController(int IN1, int IN2, int IN3, int IN4)
    {
        _motorController.CleanUp();
        _motorController.Init(IN1, IN2, IN3, IN4);
        return _motorController;
    }
}