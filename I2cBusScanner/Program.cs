// See https://aka.ms/new-console-template for more information
using Iot.Device.Board;
using System.Device.I2c;

Console.WriteLine("Bus Scanner started");
var raspberryPibrd = new RaspberryPiBoard();
var controller = raspberryPibrd.CreateGpioController();

int busNo = raspberryPibrd.GetDefaultI2cBusNumber();
Console.WriteLine("Bus No:" + busNo);
var ic2Bus = I2cBus.Create(busNo);
var busItems = I2cBusExtensions.PerformBusScan(ic2Bus);

Console.WriteLine("Found Items:" + busNo);
foreach (var busItem in busItems)
{
    Console.WriteLine(busItem.ToString());

}
Console.WriteLine("Scanning for ultraborg on Bus:" + busNo);
foreach (var busItem in busItems)
{
    byte[] recBuffer = new byte[4];
    var deviceItem = ic2Bus.CreateDevice(busItem);
    try
    {
        Console.WriteLine($"Writting to device: {busItem}");
        deviceItem.WriteByte(0x99);
        deviceItem.Read(recBuffer);
        Console.WriteLine($"Adress {busItem}: result : {recBuffer[1]}");
        if (recBuffer[1]==0x36)
            Console.WriteLine($"Ultraborg found on address: {busItem}");
    }
    catch
    {
        Console.WriteLine($"result : failed");
    }

}
Console.ReadLine();