using Iot.Device.Board;
using PanTiltHATTest;
using System.Device.I2c;








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

for (int i = 10; i <= 170; i+=2)
{
    pwm.SetRotationAngle(1, i);
    if (i < 80)
    {
        pwm.SetRotationAngle(0, i);
        Console.WriteLine("Rotated to Angle:" + i);

    }
      
    Thread.Sleep(100);
}


for (int i = 170; i >= 10; i-=2)
{
    pwm.SetRotationAngle(1, i);
    if (i < 80)
    {
        pwm.SetRotationAngle(0, i);
        Console.WriteLine("Rotated to Angle:" + i);
    }
        
    Thread.Sleep(100);
}

pwm.ExitPCA9685();

Console.WriteLine("done.");

//foreach (var busItem in busItems)
//{
//    var settings = new I2cConnectionSettings(busNo, busItem);
//    var ubI2C = I2cDevice.Create(settings);
//    Console.WriteLine("Device Address:" + ubI2C.ConnectionSettings.DeviceAddress);

//}





