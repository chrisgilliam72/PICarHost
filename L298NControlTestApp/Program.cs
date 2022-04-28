// See https://aka.ms/new-console-template for more information

using Iot.Device.Board;
using PICarServerLib;

var raspberryPibrd = new RaspberryPiBoard();
var controller = raspberryPibrd.CreateGpioController();
Console.WriteLine("Pin Numbering Scheme:+"+raspberryPibrd.DefaultPinNumberingScheme);
var motorCntrller = new L298NMotorProcessor(controller);
motorCntrller.IN1 = 23;
motorCntrller.IN2 = 24;
motorCntrller.Init();
while (true)
{
    var key = Console.ReadKey();
    switch (key.KeyChar)
    {
        case '1':Console.WriteLine("faster");motorCntrller.Faster();break;
        case '2':Console.WriteLine("slower");motorCntrller.Slower();break;       
        case 'f': Console.WriteLine("Forward"); motorCntrller.Forward(); break;
        case 'b': Console.WriteLine("Backwards");motorCntrller.Back(); break;
        case 's': Console.WriteLine("Stop"); motorCntrller.Stop();break;
        case 'u':motorCntrller.Status();break;
        case 'x': Environment.Exit(0);break ;
    }
}
