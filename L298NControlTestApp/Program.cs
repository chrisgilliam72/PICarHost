// See https://aka.ms/new-console-template for more information

using Iot.Device.Board;
using PICarServerLib;

var raspberryPibrd = new RaspberryPiBoard();
var controller = raspberryPibrd.CreateGpioController();
Console.WriteLine("Pin Numbering Scheme:+"+raspberryPibrd.DefaultPinNumberingScheme);
var motorCntrller = new L298NMotorProcessor(controller,23,24,20,21);

Console.WriteLine($"Controller  IN1= {motorCntrller.IN1} In2={motorCntrller.IN2} PWN Left= {motorCntrller.PWMLChannel}");
Console.WriteLine($"Controller  IN3= {motorCntrller.IN3} In4={motorCntrller.IN4} PWN Right= {motorCntrller.PWMRChannel}");
motorCntrller.Init(); 

while (true)
{
    var speedFactor = motorCntrller.SpeedFactor;
    var key = Console.ReadKey();
    switch (key.KeyChar)
    {
        case '1':Console.WriteLine("faster"); motorCntrller.UpdateSpeedFactor(speedFactor - 0.1); break;
        case '2':Console.WriteLine("slower"); motorCntrller.UpdateSpeedFactor(speedFactor+0.1); break;       
        case 'f': Console.WriteLine("Forward"); motorCntrller.StartForward(); ; break;
        case 'b': Console.WriteLine("Backwards"); motorCntrller.StartBackwards(); break;
        case 's': Console.WriteLine("Stop"); motorCntrller.Stop(); break;
        case 'x': motorCntrller.CleanUp(); Environment.Exit(0);break ;
    }
}
