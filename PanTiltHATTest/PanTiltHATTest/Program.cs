using Iot.Device.Board;
using PanTiltHATTest;
using System.Device.I2c;
//https://neilsnotes.net/Software/Coding/dotnetPiCamServer.html

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Scanning bus ");
var raspberryPibrd = new RaspberryPiBoard();
int busNo = raspberryPibrd.GetDefaultI2cBusNumber();
Console.WriteLine("Bus No:" + busNo);
var ic2Bus = I2cBus.Create(busNo);
var busItems = I2cBusExtensions.PerformBusScan(ic2Bus);

Console.WriteLine(("No Bus items: " + busItems.Count));

Console.WriteLine("Opening IC2 Connection...");

Console.WriteLine("Testing");
var pwm = new PCA9685();
pwm.Init(busNo, 0x40);
pwm.SetPWMFreq(50);
pwm.SetRotationAngle(1, 0);
while (true)
{
    for (int i = 10; i < 170; i++)
    {
        pwm.SetRotationAngle(1, i);
        if (i < 80)
            pwm.SetRotationAngle(0, i);
        Thread.Sleep(100);

    }

    for (int j = 170; j > 10; j--)
    {
        pwm.SetRotationAngle(1, j);
        if (j < 80)
            pwm.SetRotationAngle(0, j);
        Thread.Sleep(100);
    }
    var keyPress = Console.ReadKey();
    if (keyPress.KeyChar == 'X')
        break;
}

Console.WriteLine("Exiting...");
pwm.ExitPCA9685();




