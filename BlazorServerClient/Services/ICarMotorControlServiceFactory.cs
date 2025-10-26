using L298NLibrary;
internal interface ICarMotorControlServiceFactory
{
    public IMotorController CreateMotorController(int IN1, int IN2, int IN3, int IN4);
}